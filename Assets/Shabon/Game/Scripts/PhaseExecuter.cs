#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Shabon.Bubble;
using Shabon.Score;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using R3;
using Shabon.Utility;
using Shabon.Param;
using Shabon.Clap;
using Shabon.Breath;
using Shabon.Tutorial;
using System.Media;
using Shabon.Sound;

namespace Shabon.Game
{
    /// <summary>
    /// ゲームのフェーズを実行するクラス
    /// </summary>
    public class PhaseExecutor : ITickable, IGameState, IDisposable
    {
        // 現在の状態
        public GameState CurrentState => _currentState;
        private GameState _currentState = GameState.None;


        public double CurrentTime => _currentTime; // 現在の時間を公開
        public double LastPhaseUpdateTime => _phaseUpdatedTime; // 最後にフェーズが更新された時間
        public double FinishedTime => _phaseUpdatedTime + _gamePhases.GetCurrentPhaseData().PhaseLengthTime; // フェーズ終了時間を計算
        public IGamePhases GamePhases => _gamePhases; // 公開プロパティを追加

        private readonly IGamePhases _gamePhases;   // ゲームフェーズに関するデータ用のクラス
        private readonly IBubbleSpawner _bubbleSpawner; // バブル生成用のクラス
        private readonly IDirtValue _dirtValue;    // 汚れ値
        private readonly IScoreValue _scoreValue;  // スコア値
        private readonly IBubbleCombo _bubbleCombo; // コンボ値
        private readonly BubbleCluster _bubbleCluster;
        private readonly ClapModel _clapModel;
        private readonly BreathModel _breathModel;
        private readonly IGameRuleParam _gameRuleParam;



        private double _currentTime;    // 現在の時間
        private double _phaseUpdatedTime;   // フェーズが更新された時間
        private int _bubbleCount;   // バブルの生成数
        public static float BossBattleStartTime = 0f; // ボスバブルの戦いが始まった時間

        private List<PhaseEvent> _eventList = new();
        private List<IDisposable> _disposables = new();
        private SoundToken? _bgmToken;


        [Inject]
        public PhaseExecutor(
            IGamePhases gamePhases,
            IBubbleSpawner bubbleSpawner,
            IDirtValue dirtValue,
            IScoreValue scoreValue,
            IBubbleCombo bubbleCombo,
            BubbleCluster bubbleCluster,
            ClapModel clapModel,
            BreathModel breathModel,
            TutorialFacilitator tutorialFacilitator,
            IGameRuleParam gameRuleParam
        )
        {
            _gamePhases = gamePhases;
            _bubbleSpawner = bubbleSpawner;
            _dirtValue = dirtValue;
            _scoreValue = scoreValue;
            _bubbleCombo = bubbleCombo;
            _bubbleCluster = bubbleCluster;
            _clapModel = clapModel;
            _breathModel = breathModel;
            _gameRuleParam = gameRuleParam;

            // 初期化
            _currentTime = 0;
            _phaseUpdatedTime = 0;
            _bubbleCount = 0;

            // チュートリアル実行
            _currentState = GameState.Tutorial;
            tutorialFacilitator.StartTutorial(() =>
            {
                _currentState = GameState.Game;

                // 使用したデータを削除
                _dirtValue.Reset();
                _clapModel.Reset();
                _breathModel.Reset();

                StartPhase();

                // BGM再生
                Observable.TimerFrame(1).
                    Subscribe(_ =>
                    {
                        _bgmToken = SoundPlayerMono.Instance?.PlayBgm(BgmTypeEnum.InGameBGM) ?? null;
                    });
            });
        }

        // フェーズ開始
        void StartPhase()
        {
            // フェーズ更新時間を更新
            _phaseUpdatedTime = _currentTime;

            // 最初の敵生成
            SubscribeSpawnBubble();

            // フェーズの終了を設定
            SubscribeFinishPhase();

            // ゲームオーバー条件
            _disposables.Add(Observable.EveryValueChanged(_dirtValue, d => d.DirtNum)
                .Subscribe(dirtNum =>
                {
                    if (dirtNum >= _gameRuleParam.MaxDirtValue)
                    {
                        // ゲームオーバー
                        _currentState = GameState.Lose;
                        SaveData();
                    }
                })
            );
        }

        // 敵を生成するイベントを登録
        void SubscribeSpawnBubble()
        {
            // 次の生成時間
            double nextTime = _currentTime + _gamePhases.GetCurrentPhaseData().SpawnBubbleInterval;

            // フェーズが終了してたら終わり
            if (nextTime > _phaseUpdatedTime + _gamePhases.GetCurrentPhaseData().PhaseLengthTime)
            {
                return;
            }

            _eventList.Add(new PhaseEvent(
                nextTime,
                () =>
                {
                    _bubbleCount++;
                    // 一度の生成数だけバブルを生成
                    for (int i = 0; i < _gamePhases.GetCurrentPhaseData().BubblesPerSpawn; i++)
                    {
                        // バブルの最大数を超えないようにする
                        if (_bubbleCluster.Bubbles.Count() < _gamePhases.GetCurrentPhaseData().MaxBabbleOnField)
                        {
                            // ランダムにBubbleスポーン
                            BubbleType bubbleType = SelectSpawningBubble(_gamePhases.GetCurrentPhaseData().SpawningBubbles);

                            // ボスバブルが初めて出現したタイミングで_bossBattleStartTimeをセット
                            if (bubbleType == BubbleType.Boss && BossBattleStartTime == 0f)
                            {
                                BossBattleStartTime = Time.time;
                            }

                            _bubbleSpawner.Spawn(bubbleType);
                        }
                    }

                    // 次のEventを登録
                    SubscribeSpawnBubble();
                }
            ));
        }

        // フェーズに関するイベント
        void SubscribeFinishPhase()
        {
            double finishedTime = _currentTime + _gamePhases.GetCurrentPhaseData().PhaseLengthTime;
            // フェーズの終了を登録
            _eventList.Add(new PhaseEvent(
                finishedTime,
                () =>
                {
                    // 次のフェーズに
                    bool isEnd = _gamePhases.Proceed();
                    // 敵がいなくなるのを待つ
                    Observable.EveryUpdate()
                    .SkipWhile(_ => { return _bubbleCluster.Bubbles.Any(); })   // バブルがいなくなるまで待機
                    .Take(1)                                                    // 一度だけ実行
                    .Subscribe(_ =>
                    {
                        if (isEnd)
                        {
                            // ボスバトル時間を計算
                            float bossBattleTime = 0f;
                            if (BossBattleStartTime > 0f)
                            {
                                bossBattleTime = (float)(_currentTime - BossBattleStartTime);
                            }


                            ResultData.SaveResults(
                                _dirtValue.DirtNum,                       // FinalDirt
                                _bubbleCombo.MaxNum,                      // FinalCombo
                                _clapModel.ClapCount,                     // FinalClapCount
                                (_dirtValue as DirtValue)?.IncreaseCount ?? 0, // DirtValueCountSum（増加回数）
                                _breathModel.TotalBreathTime,             // FinalBreathTime
                                _breathModel.TotalBreathStrength,         // Calorie計算用
                                bossBattleTime                            // BossBattleTime
                            );


                            SaveData(bossBattleTime);


                            RankingSceneDataGenerator.GenerateRankingSceneData();

                            // スコアを保存
                            // RankingScore.SaveScore(_scoreValue.ScoreNum);

                            // 勝った
                            _currentState = GameState.Win;
                        }
                        else
                        {
                            StartPhase();   // 次のフェーズ
                        }
                    });
                }
            ));
        }

        private void SaveData(float bossBattleTime = -1)
        {
            ResultData.SaveResults(
                                _dirtValue.DirtNum,
                                _scoreValue.ScoreNum,
                                _bubbleCombo.MaxNum,
                                _clapModel.ClapCount,
                                _dirtValue.DecreaseCount,
                                _breathModel.TotalBreathTime,
                                _breathModel.TotalBreathStrength,
                                bossBattleTime
                            );
        }

        void ITickable.Tick()
        {
            _currentTime += Time.deltaTime; // 現在の時間を更新

            InvokeEvent();

            if (UnityEngine.Input.GetKeyDown(KeyCode.O))
            {
                _currentState = GameState.Lose;
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.C))
            {
                _currentState = GameState.Win;
            }
        }

        // 呼び出し時間がきているイベントを呼び出す
        void InvokeEvent()
        {
            // 呼び出し時間がきているEventを取得
            var invokableEvents = _eventList.Where(e => e.EventTime <= _currentTime);

            // 空なら早期Return Count()を使うと遅い場合がある
            if (!invokableEvents.Any())
            {
                return;
            }
            // 時間順に並びかえ
            invokableEvents = invokableEvents.OrderBy(e => e.EventTime);

            foreach (PhaseEvent phaseEvent in invokableEvents)
            {
                // イベント実行
                phaseEvent.Event.Invoke();

                // 実行したものをリストから消去
                _eventList.Remove(phaseEvent);
            }
        }

        /// <summary>
        /// 時間に応じて発生させられるイベント
        /// </summary>
        private class PhaseEvent
        {
            public double EventTime { get; }    // 発生時間
            public Action Event { get; }    // 発生イベント

            public PhaseEvent(double time, Action event_)
            {
                EventTime = time;
                Event = event_;
            }
        }

        /// <summary>
        /// 生成されるバブルを確率で選択
        /// </summary>
        /// <param name="spawningRatios"></param>
        /// <returns></returns>
        public BubbleType SelectSpawningBubble(IEnumerable<SpawningRatio> spawningRatios)
        {
            float whole = 0;    // 全体の割合
            foreach (var spawningRatio in spawningRatios)
            {
                whole += spawningRatio.Ratio;
            }

            // 生成されるバブルを確率で選択
            float rand = UnityEngine.Random.Range(0, whole);

            float sum = 0;  // 合計値
            foreach (var spawningRatio in spawningRatios)
            {
                sum += spawningRatio.Ratio;
                if (rand <= sum)
                {
                    return spawningRatio.Type; // 選択されたバブルタイプを返す
                }
            }

            return BubbleType.Normal; // デフォルトはNormalバブル
        }

        void IDisposable.Dispose()
        {
            // BGMとめる
            if (_bgmToken != null)
            {
                SoundPlayerMono.Instance?.StopSound(_bgmToken);
            }

            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}
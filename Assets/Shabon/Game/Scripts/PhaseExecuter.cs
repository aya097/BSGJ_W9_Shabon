#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Shabon.Bubble;
using Unity.VisualScripting;
using Shabon.Score;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Shabon.Game
{
    /// <summary>
    /// ゲームのフェーズを実行するクラス
    /// </summary>
    public class PhaseExecutor : ITickable
    {
        private readonly IGamePhases _gamePhases;   // ゲームフェーズに関するデータ用のクラス
        private readonly IBubbleSpawner _bubbleSpawner; // バブル生成用のクラス
        private readonly IDirtValue _dirtValue;    // 汚れ値
        private readonly IScoreValue _scoreValue;  // スコア値
        private readonly IBubbleCombo _bubbleCombo; // コンボ値

        private double _currentTime;    // 現在の時間
        private int _bubbleCount;   // バブルの生成数

        private List<PhaseEvent> _eventList = new();

        [Inject]
        public PhaseExecutor(
            IGamePhases gamePhases,
            IBubbleSpawner bubbleSpawner,
            IDirtValue dirtValue,
            IScoreValue scoreValue,
            IBubbleCombo bubbleCombo)
        {
            _gamePhases = gamePhases;
            _bubbleSpawner = bubbleSpawner;
            _dirtValue = dirtValue;
            _scoreValue = scoreValue;
            _bubbleCombo = bubbleCombo;

            // 初期化
            _currentTime = 0;
            _bubbleCount = 0;

            // 最初の敵生成
            SubscribeSpawnBubble();
        }

        // 敵を生成するイベントを登録
        void SubscribeSpawnBubble()
        {
            _eventList.Add(new PhaseEvent(
                _currentTime + _gamePhases.GetCurrentPhaseData().SpawnBubbleInterval,
                () =>
                {
                    _bubbleCount++;

                    // Bubbleスポーン
                    _bubbleSpawner.Spawn(BubbleType.Normal);

                    // 次のEventを登録
                    SubscribeSpawnBubble();
                }
            ));
        }

        void ITickable.Tick()
        {
            _currentTime += Time.deltaTime;

            InvokeEvent();
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


    }
}
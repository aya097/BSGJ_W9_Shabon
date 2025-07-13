#nullable enable

using System;
using System.Collections.Generic;
using R3;
using Shabon.Bubble;
using Shabon.Clap;
using Shabon.Game;
using Shabon.Input;
using Shabon.Param;
using Shabon.Score;
using Shabon.Utility;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Shabon.Ui
{
    /// <summary>
    /// インゲームのUI用Presenter
    /// </summary>
    public class InGameUiPresenter : IInitializable, IDisposable
    {
        private List<IDisposable> _disposables = new(); // R3用

        // リザルトからタイトルへ遷移できるか
        private bool _ableTransitionTitle = false;
        // 遷移を許可する時間
        private float _waitTransitionTime = 3f;
        [Inject]
        public InGameUiPresenter(
            // Model
            IDirtValue dirtValue,
            IBubbleCombo bubbleCombo,
            PhaseExecutor phaseExecutor,
            ClapModel clapModel,
            IGameRuleParam gameRuleParam,
            // View
            DirtValueViewMono dirtValueViewMono,
            ComboSpawner comboSpawner,   // ViewではないがViewを生成する
            ClockViewMono clockViewMono,
            ClapUiViewMono clapUiViewMono,
            ResultViewMono resultViewMono,
            IInputManager inputManager
        )
        {
            // View -> Model
            // リザルトからタイトルへ
            _disposables.Add(Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    // リザルト状態
                    if (phaseExecutor.CurrentState == GameState.Win || phaseExecutor.CurrentState == GameState.Lose)
                    {
                        if (_ableTransitionTitle)
                        {
                            if (inputManager.GetClap())
                            {
                                // タイトル戻る
                                SceneTransition.Transition(SceneName.TitleScene);
                            }
                        }

                    }
                })
            );

            // Model -> View
            // 汚れ値をUiに反映
            _disposables.Add(
                Observable.EveryValueChanged(dirtValue, d => d.DirtNum)
                .Subscribe(value =>
                {
                    dirtValueViewMono.SetValue(value, 0, gameRuleParam.MaxDirtValue);
                })
            );

            // コンボ完了でUIに反映（spawn）
            _disposables.Add(
                Observable.EveryValueChanged(bubbleCombo, b => b.IsCombo)
                .Subscribe(isCombo =>
                {
                    if (isCombo == false && bubbleCombo.ComboNum > 0)
                    {
                        comboSpawner.Spawn(bubbleCombo.ComboNum);
                        bubbleCombo.Reset();
                    }
                })
            );

            // 時間を時計に反映
            _disposables.Add(
                Observable.EveryValueChanged(phaseExecutor, p => p.CurrentTime)
                .Subscribe(time =>
                {
                    clockViewMono.SetTime((float)time, (float)phaseExecutor.LastPhaseUpdateTime, (float)phaseExecutor.FinishedTime);
                })
            );

            // Clapのクールタイム
            _disposables.Add(
                Observable.EveryValueChanged(clapModel, c => c.CurrentTime)
                .Subscribe(time =>
                {
                    clapUiViewMono.SetCoolTime(0, clapModel.CoolTime, clapModel.CurrentTime);
                })
            );

            // リザルト表示
            _disposables.Add(
                Observable.EveryValueChanged(phaseExecutor, p => p.CurrentState)
                .Subscribe(state =>
                {
                    if (state == GameState.Win || state == GameState.Lose)
                    {
                        resultViewMono.Open();
                        // 遷移まで待機
                        Observable.Timer(TimeSpan.FromSeconds(_waitTransitionTime))
                            .Subscribe(_ =>
                            {
                                _ableTransitionTitle = true;
                            });
                    }
                })
            );
        }

        // このクラスを生成するためのエントリーポイント
        void IInitializable.Initialize()
        {

        }

        void IDisposable.Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}
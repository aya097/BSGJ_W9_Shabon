#nullable enable

using System;
using System.Collections.Generic;
using R3;
using Shabon.Bubble;
using Shabon.Game;
using Shabon.Param;
using Shabon.Score;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Shabon.Ui
{
    /// <summary>
    /// インゲームのUI用Presenter
    /// </summary>
    public class InGameUiPresenter : IInitializable
    {
        private List<IDisposable> _disposables = new(); // R3用

        [Inject]
        public InGameUiPresenter(
            // Model
            IDirtValue dirtValue,
            IBubbleCombo bubbleCombo,
            PhaseExecutor phaseExecutor,
            // View
            DirtValueViewMono dirtValueViewMono,
            ComboSpawner comboSpawner,   // ViewではないがViewを生成する
            ClockViewMono clockViewMono
        )
        {
            // Model -> View
            // 汚れ値をUiに反映
            _disposables.Add(
                Observable.EveryValueChanged(dirtValue, d => d.DirtNum)
                .Subscribe(value =>
                {
                    dirtValueViewMono.SetValue(value, 0, 10);   // 仮で min = 0, max = 10
                })
            );

            // コンボが0以外に変化したら生成
            Observable.EveryValueChanged(bubbleCombo, b => b.ComboNum)
            .Subscribe(value =>
            {
                if (value != 0)
                {
                    comboSpawner.Spawn(value);
                }
            });

            // 時間を時計に反映
            Observable.EveryValueChanged(phaseExecutor, p => p.CurrentTime)
                .Subscribe(time =>
                {
                    Debug.Log($"{time}");
                    clockViewMono.SetTime((float)time, (float)phaseExecutor.LastPhaseUpdateTime, (float)phaseExecutor.FinishedTime);
                });

        }

        // このクラスを生成するためのエントリーポイント
        void IInitializable.Initialize()
        {

        }
    }
}
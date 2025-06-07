#nullable enable

using System;
using System.Collections.Generic;
using R3;
using Shabon.Score;
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
            // View
            DirtValueViewMono dirtValueViewMono
        )
        {
            // Model -> View
            _disposables.Add(
                Observable.EveryValueChanged(dirtValue, d => d.DirtNum)
                .Subscribe(value =>
                {
                    dirtValueViewMono.SetValue(value, 0, 10);   // 仮で min = 0, max = 10
                })
            );

        }

        // このクラスを生成するためのエントリーポイント
        void IInitializable.Initialize()
        {

        }
    }
}
#nullable enable

using UnityEngine;
using Shabon.Utility;
using VContainer;
using VContainer.Unity;
using Shabon.Input;
using R3;
using System;

namespace Shabon.Title
{
    /// <summary>
    /// タイトルシーンのViewとModelを橋渡しするクラス
    /// </summary>
    public class TitlePresenter : IInitializable, IDisposable
    {
        IDisposable _disposable;
        [Inject]
        public TitlePresenter(TitleViewMono titleViewMono, IInputManager inputManager)
        {
            // Model -> View
            // 今回はなし

            // View -> Model

            // プロローグが終われば、シーン遷移
            titleViewMono.ProloguePlayableDirector.stopped +=
                director => SceneTransition.Transition(SceneName.GameScene);

            // 入力されたら押されたら、プロローグ開始
            _disposable = Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    if (inputManager.GetClap())
                    {
                        titleViewMono.StartPrologue();
                    }
                });
        }

        // このクラスを生成するためのエントリーポイント
        void IInitializable.Initialize()
        {

        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }
    }
}
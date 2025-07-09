#nullable enable

using UnityEngine;
using Shabon.Utility;
using VContainer;
using VContainer.Unity;

namespace Shabon.Title
{
    /// <summary>
    /// タイトルシーンのViewとModelを橋渡しするクラス
    /// </summary>
    public class TitlePresenter : IInitializable
    {
        [Inject]
        public TitlePresenter(TitleViewMono titleViewMono)
        {
            // Model -> View
            // 今回はなし

            // View -> Model

            // プロローグが終われば、シーン遷移
            titleViewMono.ProloguePlayableDirecctor.stopped +=
                director => SceneTransition.Transition(SceneName.GameScene);

            // ボタン押されたら、プロローグ開始
            titleViewMono.StartButton.onClick.AddListener(titleViewMono.StartPrologue);
        }

        // このクラスを生成するためのエントリーポイント
        void IInitializable.Initialize()
        {

        }
    }
}
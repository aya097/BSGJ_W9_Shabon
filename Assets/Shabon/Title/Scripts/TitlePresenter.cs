#nullable enable

using Shabon.Utility;

namespace Shabon.Title
{
    /// <summary>
    /// タイトルシーンのViewとModelを橋渡しするクラス
    /// </summary>
    public class TitlePresenter
    {
        public TitlePresenter(TitleViewMono titleViewMono)
        {
            // Model -> View
            // 今回はなし

            // View -> Model

            // スタートボタンを押して、シーン遷移
            titleViewMono.StartButton.onClick.AddListener(() =>
            {
                SceneTransition.Transition(SceneName.GameScene);
            });
        }
    }
}
#nullable enable

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Shabon.Title
{
    /// <summary>
    /// タイトルシーンのビュークラス
    /// </summary>
    public class TitleViewMono : MonoBehaviour
    {
        [SerializeField] private RawImage titleImage = null!;
        [SerializeField] private Button startButton = null!;
        [SerializeField] private GameObject prologue = null!;
        [SerializeField] private PlayableDirector prologuePlayableDirecctor = null!;

        // ゲッター
        public Button StartButton => startButton;
        public PlayableDirector ProloguePlayableDirecctor => prologuePlayableDirecctor;

        // プロローグを再生するメソッド
        public void StartPrologue()
        {
            // 初期UIを非表示に
            titleImage.enabled = false;
            startButton.gameObject.SetActive(false);

            // プロローグの準備
            prologue.SetActive(true);
            prologuePlayableDirecctor.Play();
        }
    }
}

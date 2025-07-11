#nullable enable

using Shabon.Sound;
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


        // BGM停止用
        private SoundToken _bgmToken = null!;
        void Start()
        {
            // BGM再生
            _bgmToken = SoundPlayerMono.Instance?.PlayBgm(BgmTypeEnum.TitleBGM) ?? null!;
        }
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

        void OnDestroy()
        {
            // BGM止める
            if (_bgmToken != null)
            {
                SoundPlayerMono.Instance?.StopSound(_bgmToken);
            }
        }
    }
}

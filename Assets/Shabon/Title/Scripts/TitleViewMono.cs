#nullable enable

using LitMotion;
using LitMotion.Extensions;
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
        [Header("タイトル")]
        [SerializeField] private Image bubble0 = null!;
        [SerializeField] private float bubble0MoveAmount;
        [SerializeField] private float bubble0Duration;
        [SerializeField] private Image bubble1 = null!;
        [SerializeField] private float bubble1MoveAmount;
        [SerializeField] private float bubble1Duration;
        // [SerializeField] private Image bubble1 = null!;
        // [SerializeField] private Image bubble2 = null!;
        // [SerializeField] private Image bubble3 = null!;

        [Header("プロローグ")]
        [SerializeField] private RawImage titleImage = null!;
        [SerializeField] private Button startButton = null!;
        [SerializeField] private GameObject prologue = null!;
        [SerializeField] private PlayableDirector prologuePlayableDirecctor = null!;

        // ゲッター
        public Button StartButton => startButton;
        public PlayableDirector ProloguePlayableDirecctor => prologuePlayableDirecctor;


        // BGM停止用
        private SoundToken _bgmToken = null!;

        // バブルアニメーション用
        private Vector3 _originalBubble0Position;
        private Vector3 _originalBubble1Position;
        void Start()
        {
            // BGM再生
            _bgmToken = SoundPlayerMono.Instance?.PlayBgm(BgmTypeEnum.TitleBGM) ?? null!;

            // バブルのアニメーション
            _originalBubble0Position = bubble0.GetComponent<RectTransform>().anchoredPosition;

            LMotion.Create(_originalBubble0Position.y, _originalBubble0Position.y + bubble0MoveAmount, bubble0Duration)
                .WithEase(Ease.OutQuad)
                .WithDelay(0.5f, DelayType.EveryLoop)
                .WithLoops(-1, LoopType.Flip)
                .BindToAnchoredPosition3DY(bubble0.GetComponent<RectTransform>());

            _originalBubble1Position = bubble1.GetComponent<RectTransform>().anchoredPosition;

            LMotion.Create(_originalBubble1Position.y, _originalBubble1Position.y + bubble1MoveAmount, bubble1Duration)
                .WithEase(Ease.OutQuad)
                .WithDelay(0.5f, DelayType.EveryLoop)
                .WithLoops(-1, LoopType.Flip)
                .BindToAnchoredPosition3DY(bubble1.GetComponent<RectTransform>());
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

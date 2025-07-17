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
        [SerializeField] private GameObject titleBack = null!;
        [SerializeField] private GameObject titleLogo = null!;
        [SerializeField] private Image bubble0 = null!;
        [SerializeField] private float bubble0MoveAmount;
        [SerializeField] private float bubble0Duration;
        [SerializeField] private Image bubble1 = null!;
        [SerializeField] private float bubble1MoveAmount;
        [SerializeField] private float bubble1Duration;

        [Header("プロローグ")]
        [SerializeField] private GameObject prologue = null!;
        [SerializeField] private PlayableDirector prologuePlayableDirector = null!;

        // ゲッター

        public PlayableDirector ProloguePlayableDirector => prologuePlayableDirector;


        // BGM停止用
        private SoundToken _titleBgmToken = null!;
        private SoundToken _prologueBgmToken = null!;

        // バブルアニメーション用
        private Vector3 _originalBubble0Position;
        private Vector3 _originalBubble1Position;
        void Start()
        {
            // BGM再生
            _titleBgmToken = SoundPlayerMono.Instance?.PlayBgm(BgmTypeEnum.TitleBGM) ?? null!;

            // バブルのアニメーション
            _originalBubble0Position = bubble0.GetComponent<RectTransform>().anchoredPosition;

            LMotion.Create(_originalBubble0Position.y, _originalBubble0Position.y + bubble0MoveAmount, bubble0Duration)
                .WithEase(Ease.OutQuad)
                .WithDelay(0.5f, DelayType.EveryLoop)
                .WithLoops(-1, LoopType.Flip)
                .BindToAnchoredPosition3DY(bubble0.GetComponent<RectTransform>())
                .AddTo(this);

            _originalBubble1Position = bubble1.GetComponent<RectTransform>().anchoredPosition;

            LMotion.Create(_originalBubble1Position.y, _originalBubble1Position.y + bubble1MoveAmount, bubble1Duration)
                .WithEase(Ease.OutQuad)
                .WithDelay(0.5f, DelayType.EveryLoop)
                .WithLoops(-1, LoopType.Flip)
                .BindToAnchoredPosition3DY(bubble1.GetComponent<RectTransform>())
                .AddTo(this);
        }
        // プロローグを再生するメソッド
        public void StartPrologue()
        {
            // BGM止める
            if (_bgmToken != null)
            {
                SoundPlayerMono.Instance?.StopSound(_bgmToken);
                _bgmToken = SoundPlayerMono.Instance?.PlayBgm(BgmTypeEnum.PrologueBGM) ?? null!; // prologueのBGM再生
            }

            titleBack.SetActive(false);
            titleLogo.SetActive(false);

            // プロローグの準備
            prologue.SetActive(true);
            prologuePlayableDirector.Play();

            // BGM切り替え
            if (_titleBgmToken != null)
            {
                SoundPlayerMono.Instance?.StopSound(_titleBgmToken);
            }
            _prologueBgmToken = SoundPlayerMono.Instance?.PlayBgm(BgmTypeEnum.Prologue) ?? null!;

        }

        void OnDestroy()
        {
            // BGM止める
            if (_titleBgmToken != null)
            {
                SoundPlayerMono.Instance?.StopSound(_titleBgmToken);
            }
            if (_prologueBgmToken != null)
            {
                SoundPlayerMono.Instance?.StopSound(_prologueBgmToken);
            }
        }
    }
}

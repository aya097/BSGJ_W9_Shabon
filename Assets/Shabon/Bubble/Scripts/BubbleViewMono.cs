#nullable enable
using System;
using R3;
using R3.Triggers;
using UnityEngine;
using LitMotion;
using LitMotion.Extensions;
using Shabon.Sound;

namespace Shabon.Bubble
{
    public enum BubbleAnimationEnum
    {
        Idle,
        Clap,
        Breath,
        Attack,
        Spawn
    }
    public enum HighLightType
    {
        None,
        Clapable,
        Claped,
        Attack,
        Breathed,
    }
    /// <summary>
    /// Bubbleの見た目を管理するクラス
    /// </summary>
    public class BubbleViewMono : MonoBehaviour
    {
        [SerializeField] protected Animator _bubbleAnimator = null!;
        [SerializeField] protected SpriteRenderer _spriteRenderer = null!;

        [Header("Clapされたときのエフェクト")]
        [SerializeField] protected GameObject cureEffect = null!;

        [Header("影")]
        [SerializeField] protected GameObject shadow = null!;

        protected BubbleAnimationEnum _currentAnimation = BubbleAnimationEnum.Idle;
        private IDisposable? _breathDisposable = null!;
        protected Color _originalColor;
        private Vector3 _originalShadowScale;
        private BubbleType _bubbleType = BubbleType.None;
        private float _originalShadowDistance; // 影の元の距離

        private SoundToken _breathedToken = null!;

        void Awake()
        {
            _originalColor = _spriteRenderer.color;
            cureEffect.SetActive(false);
            _originalShadowScale = shadow.transform.localScale;
            _originalShadowDistance = Mathf.Abs(shadow.transform.localPosition.y);

            Vector3 startPosition = transform.localPosition;
            LMotion.Create(startPosition.y, startPosition.y + 0.03f, 0.8f)
                .WithEase(Ease.InOutSine)
                .WithLoops(-1, LoopType.Yoyo)
                .BindToLocalPositionY(transform)
                .AddTo(gameObject);
        }

        void Update()
        {
            // 影の位置を調整
            // 真下にRaycastして、ヒットした位置に影を配置
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Stage")))
            {
                // ヒットした位置に影を配置
                float shadowYPos = hit.point.y + 0.01f; // 少し上に配置
                shadow.transform.position = new Vector3(transform.position.x, shadowYPos, transform.position.z);

                // 距離に応じてサイズ変更
                float distance = (transform.position - hit.point).magnitude;
                shadow.transform.localScale = _originalShadowScale * distance / _originalShadowDistance * 1.5f;
            }
        }
        public void SetBubbleType(BubbleType bubbleType)
        {
            _bubbleType = bubbleType;
        }


        // ハイライトを設定
        public void SetHighlight(HighLightType highLightType)
        {
            if (highLightType == HighLightType.Clapable)
            {
                SetDarkness(0f);
                TurnOnHighlight();
            }
            else if (highLightType == HighLightType.Claped)
            {
                SetDarkness(0f);
                TurnOffHighlight();
            }
            else if (highLightType == HighLightType.Attack)
            {
                SetDarkness(0.2f);
                TurnOffHighlight();
            }
            else if (highLightType == HighLightType.Breathed)
            {
                SetDarkness(0f);
            }
            else if (highLightType == HighLightType.None)
            {
                SetDarkness(0f);
                TurnOffHighlight();
            }
        }
        // ハイライト
        protected virtual void TurnOnHighlight()
        {
            _spriteRenderer.material.SetFloat("_HighLightFlag", 1f);
        }
        protected virtual void TurnOffHighlight()
        {
            _spriteRenderer.material.SetFloat("_HighLightFlag", 0f);
        }
        // メイド
        protected virtual void SetDarkness(float value)
        {
            _spriteRenderer.color = _originalColor - new Color(value, value, value, 0f);
        }




        // 息吹かれたときのアニメーションを再生するメソッド
        public void PlayBreath(IBubbleMono bubbleMono)
        {
            // Breathは毎フレーム呼ばれるから修正
            _breathDisposable?.Dispose();

            // サウンドを再生
            if (_breathedToken == null)
            {
                var seType = GetBreathedSe(_bubbleType);
                _breathedToken = SoundPlayerMono.Instance?.PlaySe(seType) ?? null!;
            }
            _breathDisposable = Observable.Timer(TimeSpan.FromSeconds(0.1f))
                .Subscribe(_ =>
                {
                    Play(BubbleAnimationEnum.Idle);
                    bubbleMono.IsBreathing = false;
                    // サウンド中止
                    if (_breathedToken != null)
                    {
                        SoundPlayerMono.Instance?.StopSound(_breathedToken);
                        _breathedToken = null!;
                    }
                });

            Play(BubbleAnimationEnum.Breath);
        }

        //攻撃するときのアニメーションを再生するメソッド
        public virtual void PlayAttack(Action? callback = null)
        {
            // Breathをリセット
            _breathDisposable?.Dispose();

            Play(BubbleAnimationEnum.Attack);
            Observable.Timer(TimeSpan.FromSeconds(0.7f))
                .Subscribe(_ =>
                {
                    callback?.Invoke();
                }).AddTo(this);

            // 影をなくす
            shadow.SetActive(false);
        }

        // Clapされたときのアニメーション
        public virtual void PlayClap(Action? callback = null)
        {
            // Armorはまだないので 
            if (_bubbleType == BubbleType.Armor)
            {
                SoundPlayerMono.Instance?.PlaySe(SeTypeEnum.ArmorBubbleClaped);
                return;
            }

            // Breathをリセット
            _breathDisposable?.Dispose();

            Play(BubbleAnimationEnum.Clap);
            Observable.Timer(TimeSpan.FromSeconds(0.9f))
                .Subscribe(_ =>
                {
                    callback?.Invoke();
                }).AddTo(this);

            // CureEffectを再生
            cureEffect.SetActive(true);
            // 影をなくす
            shadow.SetActive(false);
        }



        protected virtual void Play(BubbleAnimationEnum animation)
        {
            if (_currentAnimation != animation)
            {
                _currentAnimation = animation;
                _bubbleAnimator.SetTrigger(animation.ToString());
            }
        }

        protected SeTypeEnum GetBreathedSe(BubbleType bubbleType)
        {
            return bubbleType switch
            {
                BubbleType.Normal => SeTypeEnum.NormalBubbleBreathed,
                BubbleType.Quick => SeTypeEnum.QuickBubbleBreathed,
                BubbleType.Boss => SeTypeEnum.BossBubbleBreathed,
                _ => SeTypeEnum.NormalBubbleBreathed
            };
        }

        void OnDestroy()
        {
            if (_breathedToken != null)
            {
                SoundPlayerMono.Instance?.StopSound(_breathedToken);
            }
            _breathDisposable?.Dispose();
        }
    }
}
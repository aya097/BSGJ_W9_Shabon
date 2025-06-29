#nullable enable
using System;
using R3;
using R3.Triggers;
using UnityEngine;

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

        protected BubbleAnimationEnum _currentAnimation = BubbleAnimationEnum.Idle;
        private IDisposable? _breathDisposable = null!;
        protected Color _originalColor;

        void Awake()
        {
            _originalColor = _spriteRenderer.color;
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
        public void PlayBreath()
        {
            // Breathは毎フレーム呼ばれるから修正
            _breathDisposable?.Dispose();

            _breathDisposable = Observable.Timer(TimeSpan.FromSeconds(0.1f))
                .Subscribe(_ =>
                {
                    Play(BubbleAnimationEnum.Idle);
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
        }

        // Clapされたときのアニメーション
        public virtual void PlayClap(Action? callback = null)
        {
            // Breathをリセット
            _breathDisposable?.Dispose();

            Play(BubbleAnimationEnum.Clap);
            Observable.Timer(TimeSpan.FromSeconds(0.9f))
                .Subscribe(_ =>
                {
                    callback?.Invoke();
                }).AddTo(this);
        }



        protected virtual void Play(BubbleAnimationEnum animation)
        {
            if (_currentAnimation != animation)
            {
                _currentAnimation = animation;
                _bubbleAnimator.SetTrigger(animation.ToString());
            }
        }
    }
}
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
        [SerializeField] private Animator _bubbleAnimator = null!;
        [SerializeField] private SpriteRenderer _spriteRenderer = null!;

        private BubbleAnimationEnum _currentAnimation = BubbleAnimationEnum.Idle;
        private IDisposable? _breathDisposable = null!;
        private Color _originalColor;

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
        }
        // ハイライト
        private void TurnOnHighlight()
        {
            _spriteRenderer.material.SetFloat("_HighLightFlag", 1f);
        }
        private void TurnOffHighlight()
        {
            _spriteRenderer.material.SetFloat("_HighLightFlag", 0f);
        }
        // メイド
        private void SetDarkness(float value)
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
        public void PlayAttack(Action? callback = null)
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
        public void PlayClap(Action? callback = null)
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



        private void Play(BubbleAnimationEnum animation)
        {
            if (_currentAnimation != animation)
            {
                _currentAnimation = animation;
                _bubbleAnimator.SetTrigger(animation.ToString());
            }
        }
    }
}
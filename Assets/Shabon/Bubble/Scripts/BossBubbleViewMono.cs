#nullable enable

using UnityEngine;
using R3;
using System;

namespace Shabon.Bubble
{
    public class BossBubbleViewMono : BubbleViewMono
    {
        [SerializeField] private Animator _bubbleDecorationAnimator = null!;
        [SerializeField] private SpriteRenderer _decorationSpriteRenderer = null!;

        protected override void TurnOnHighlight()
        {
            _spriteRenderer.material.SetFloat("_HighLightFlag", 1f);
            _decorationSpriteRenderer.material.SetFloat("_HighLightFlag", 1f);
        }
        protected override void TurnOffHighlight()
        {
            _spriteRenderer.material.SetFloat("_HighLightFlag", 0f);
            _decorationSpriteRenderer.material.SetFloat("_HighLightFlag", 0f);
        }
        // メイド
        protected override void SetDarkness(float value)
        {
            _spriteRenderer.color = _originalColor - new Color(value, value, value, 0f);
            _decorationSpriteRenderer.color = _originalColor - new Color(value, value, value, 0f);

        }

        public override void PlayAttack(Action? callback = null)
        {
            // Breathをリセット
            // _breathDisposable?.Dispose();

            Play(BubbleAnimationEnum.Attack);
            Observable.Timer(TimeSpan.FromSeconds(1.0f))
                .Subscribe(_ =>
                {
                    Play(BubbleAnimationEnum.Idle);
                    callback?.Invoke();
                }).AddTo(this);
        }

        public override void PlayClap(Action? callback = null)
        {
            // Breathをリセット
            //_breathDisposable?.Dispose();

            Play(BubbleAnimationEnum.Clap);
            Observable.Timer(TimeSpan.FromSeconds(0.6f))
                .Subscribe(_ =>
                {
                    callback?.Invoke();
                    Play(BubbleAnimationEnum.Idle);
                }).AddTo(this);
        }

        public void PlaySpawn(Action? callback = null)
        {
            Play(BubbleAnimationEnum.Spawn);
            Observable.Timer(TimeSpan.FromSeconds(1.0f))
                .Subscribe(_ =>
                {
                    Play(BubbleAnimationEnum.Idle);
                    callback?.Invoke();
                }).AddTo(this);
        }

        protected override void Play(BubbleAnimationEnum animation)
        {
            if (_currentAnimation != animation)
            {
                _currentAnimation = animation;
                _bubbleAnimator.SetTrigger(animation.ToString());
                _bubbleDecorationAnimator.SetTrigger(animation.ToString());
            }
        }
    }
}

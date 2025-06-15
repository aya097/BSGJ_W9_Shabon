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
    /// <summary>
    /// Bubbleの見た目を管理するクラス
    /// </summary>
    public class BubbleViewMono : MonoBehaviour
    {
        [SerializeField] private Animator _bubbleAnimator = null!;

        private BubbleAnimationEnum _currentAnimation = BubbleAnimationEnum.Idle;
        private IDisposable? _breathDisposable = null!;



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
            Play(BubbleAnimationEnum.Attack);
            Observable.Timer(TimeSpan.FromSeconds(0.7f))
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
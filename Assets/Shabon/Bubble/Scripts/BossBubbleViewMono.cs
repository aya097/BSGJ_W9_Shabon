#nullable enable

using UnityEngine;
using R3;
using System;

namespace Shabon.Bubble
{
    public class BossBubbleViewMono : BubbleViewMono
    {
        [Header("装飾品のview")]
        [SerializeField] private Animator _bubbleOrnamentAnimator = null!;
        [SerializeField] private SpriteRenderer _ornamentSpriteRenderer = null!;

        private int _ornamentOriginalOrderInLayer;

        protected override void TurnOnHighlight()
        {
            _spriteRenderer.material.SetFloat("_HighLightFlag", 1f);
            _ornamentSpriteRenderer.material.SetFloat("_HighLightFlag", 1f);
        }
        protected override void TurnOffHighlight()
        {
            _spriteRenderer.material.SetFloat("_HighLightFlag", 0f);
            _ornamentSpriteRenderer.material.SetFloat("_HighLightFlag", 0f);
        }
        // メイド
        protected override void SetDarkness(float value)
        {
            _spriteRenderer.color = _originalColor - new Color(value, value, value, 0f);
            _ornamentSpriteRenderer.color = _originalColor - new Color(value, value, value, 0f);

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

        public override void PlayClap(Action? callback = null, bool isDead = true)
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
        public override void SetSortingLayer(string sortingLayerName)
        {
            base.SetSortingLayer(sortingLayerName);
            _ornamentSpriteRenderer.sortingLayerName = sortingLayerName;
        }

        protected override void Play(BubbleAnimationEnum animation)
        {
            if (_currentAnimation != animation)
            {
                _currentAnimation = animation;
                _bubbleAnimator.SetTrigger(animation.ToString());
                _bubbleOrnamentAnimator.SetTrigger(animation.ToString());
            }
        }

        protected override void Awake()
        {
            _ornamentOriginalOrderInLayer = _ornamentSpriteRenderer.sortingOrder;
            base.Awake();
        }

        protected override void Update()
        {
            // プレイヤーに近い(zが小さい)bubbleほど、手前に表示させるように
            _ornamentSpriteRenderer.sortingOrder = (int)(-transform.position.z * 10000) + _ornamentOriginalOrderInLayer;
            base.Update();
        }
    }
}

#nullable enable

using UnityEngine;
using R3;
using System;

namespace Shabon.Bubble
{
    public class ArmorBubbleViewMono : BubbleViewMono
    {
        [Header("頭部のview")]
        [SerializeField] private Animator _bubbleHeadAnimator = null!;
        [SerializeField] private SpriteRenderer _headSpriteRenderer = null!;

        private int _headOriginalOrderInLayer;

        protected override void TurnOnHighlight()
        {
            _spriteRenderer.material.SetFloat("_HighLightFlag", 1f);
            _headSpriteRenderer.material.SetFloat("_HighLightFlag", 1f);
        }
        protected override void TurnOffHighlight()
        {
            _spriteRenderer.material.SetFloat("_HighLightFlag", 0f);
            _headSpriteRenderer.material.SetFloat("_HighLightFlag", 0f);
        }
        // メイド
        protected override void SetDarkness(float value)
        {
            _spriteRenderer.color = _originalColor - new Color(value, value, value, 0f);
            _headSpriteRenderer.color = _originalColor - new Color(value, value, value, 0f);
        }

        protected override void Awake()
        {
            _headOriginalOrderInLayer = _headSpriteRenderer.sortingOrder;
            base.Awake();
        }

        override protected void Update()
        {
            _headSpriteRenderer.sortingOrder = (int)(-transform.position.z * 100) + _headOriginalOrderInLayer;
            base.Update();
        }

        protected override void Play(BubbleAnimationEnum animation)
        {
            if (_currentAnimation != animation)
            {
                _currentAnimation = animation;
                _bubbleAnimator.SetTrigger(animation.ToString());
                _bubbleHeadAnimator.SetTrigger(animation.ToString());
            }
        }
    }
}

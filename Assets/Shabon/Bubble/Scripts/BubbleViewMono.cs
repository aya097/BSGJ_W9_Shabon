#nullable enable
using UnityEngine;

namespace Shabon.Bubble
{
    public enum BubbleAnimationEnum
    {
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

        // 割られた時のアニメーションを再生するメソッド
        public void Play(BubbleAnimationEnum animationEnum)
        {
            _bubbleAnimator.SetTrigger(animationEnum.ToString());
        }
    }
}
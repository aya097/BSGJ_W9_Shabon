#nullable enable
using UnityEngine;

/// <summary>
/// Bubbleの見た目を管理するクラス
/// </summary>
public class BubbleViewMono : MonoBehaviour
{
    [SerializeField] private Animator _bubbleAnimator = null!;

    // 割られた時のアニメーションを再生するメソッド
    public void PlayClappedAnimation()
    {
        _bubbleAnimator.SetTrigger("ClapTrigger");
    }

    // 息が吹かれた時のアニメーションを再生するメソッド
    public void PlayBreathedAnimation()
    {
        Debug.Log("息が吹かれたときのアニメーションを再生");
    }
}

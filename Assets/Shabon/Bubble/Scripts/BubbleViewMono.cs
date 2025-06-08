#nullable enable
using UnityEngine;

/// <summary>
/// Bubbleの見た目を管理するクラス
/// </summary>
[RequireComponent(typeof(Animator))]
public class BubbleViewMono : MonoBehaviour
{
    private Animator _bubbleAnimator = null!;

    public void Awake()
    {
        _bubbleAnimator = GetComponent<Animator>();
    }

    public void SetAnimatorController(RuntimeAnimatorController bubbleAnimatorController)
    {
        _bubbleAnimator.runtimeAnimatorController = bubbleAnimatorController;
    }

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

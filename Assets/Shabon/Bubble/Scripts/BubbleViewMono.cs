using UnityEditor.Animations;
using UnityEngine;
using VContainer;

/// <summary>
/// Bubbleの見た目を管理するクラス
/// </summary>
[RequireComponent(typeof(Animator))]
public class BubbleViewMono : MonoBehaviour
{
    private Animator _bubbleAnimator;

    public void Awake()
    {
        _bubbleAnimator = GetComponent<Animator>();
    }

    public void SetAnimatorController(AnimatorController bubbleAnimatorController)
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

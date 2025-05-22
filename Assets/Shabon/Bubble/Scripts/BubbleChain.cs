#nullable enable

using UnityEngine;

namespace Shabon.Bubble
{
    /// <summary>
    /// バブルの連鎖処理を行うクラス
    /// </summary>
    public class BubbleChain : IBubbleChain
    {
        /// <summary>
        /// 指定したバブルを中心に一定範囲内のバブルも連鎖して割る
        /// </summary>
        public void ExecuteBubbleChain(Transform bubbleMonoTransform, float chainRadius)
        {
            Collider[] aroudBubbleColliders = Physics.OverlapSphere(bubbleMonoTransform.position, chainRadius);
            foreach (Collider aroudBubbleCollider in aroudBubbleColliders)
            {
                IBubbleMono aroundBubbleMono = aroudBubbleCollider.gameObject.GetComponent<IBubbleMono>();
                aroundBubbleMono.InvokeOnDead();
            }
        }
    }
}


#nullable enable

using Unity.VisualScripting;
using UnityEngine;
using R3;
using System;

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
        public void ExecuteBubbleChain(IBubbleMono targetBubbleMono, float chainRadius)
        {
            Vector3 targetBubblePosition = targetBubbleMono.Transform.position;
            Collider[] aroudBubbleColliders = Physics.OverlapSphere(targetBubblePosition, chainRadius);
            foreach (Collider aroudBubbleCollider in aroudBubbleColliders)
            {
                IBubbleMono aroundBubbleMono = aroudBubbleCollider.transform.parent.gameObject.GetComponent<IBubbleMono>();

                if (aroundBubbleMono is null || targetBubbleMono == aroundBubbleMono) continue;
                Observable.Timer(TimeSpan.FromSeconds(0.5f))
                    .Subscribe(_ => aroundBubbleMono.InvokeOnDead());

                //aroundBubbleMono.InvokeOnDead();
            }
        }
    }
}


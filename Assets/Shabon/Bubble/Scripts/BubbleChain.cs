#nullable enable

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
            // 連鎖元のBubbleのPosition
            Vector3 targetBubblePosition = targetBubbleMono.Transform.position;

            // まわりのBubbleを割る処理
            Collider[] aroundColliders = Physics.OverlapSphere(targetBubblePosition, chainRadius);
            foreach (Collider aroundCollider in aroundColliders)
            {
                BubbleMono aroundBubbleMono = aroundCollider.transform.parent.gameObject.GetComponent<BubbleMono>();

                // BubbleMonoのnullチェックおよび自身のBubbleMonoの場合は飛ばす
                if (aroundBubbleMono == null || (BubbleMono)targetBubbleMono == aroundBubbleMono) continue;

                // 0.5秒後に周辺のbubbleをdestory
                // * 遅延処理いれないとbubble同士が循環参照して(おそらく)unityが落ちるので注意
                IDisposable disposable = Observable.Timer(TimeSpan.FromSeconds(0.5f))
                    .Subscribe(_ => 
                    {
                        if (aroundBubbleMono == null) return;
                        
                        aroundBubbleMono.InvokeOnDead();

                    });

                // BubbleがDestoryしたら、上記の遅延処理をdisposeさせるよう設定
                IBubbleBuildSetter aroundBubbleSetter = aroundBubbleMono;
                aroundBubbleSetter.OnDead += () =>
                {
                    disposable.Dispose();
                };

            }
        }
    }
}


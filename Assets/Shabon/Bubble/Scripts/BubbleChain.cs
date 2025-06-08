#nullable enable

using UnityEngine;
using R3;
using System;
using VContainer;
using System.Linq;
using System.Collections.Generic;

namespace Shabon.Bubble
{
    /// <summary>
    /// バブルの連鎖処理を行うクラス
    /// </summary>
    public class BubbleChain : IBubbleChain
    {
        private readonly BubbleCluster _bubbleCluster;
        private readonly IBubbleCombo _bubbleCombo;

        [Inject]
        public BubbleChain(BubbleCluster bubbleCluster, IBubbleCombo bubbleCombo)
        {
            _bubbleCluster = bubbleCluster;
            _bubbleCombo = bubbleCombo;
        }

        /// <summary>
        /// 指定したバブルを中心に一定範囲内のバブルも連鎖して割る
        /// </summary>
        public void ExecuteBubbleChain(IBubbleMono targetBubbleMono, float chainRadius)
        {
            // 連鎖元のBubbleのPosition
            Vector3 targetBubblePosition = targetBubbleMono.Transform.position;

            // 周辺のBubbleを取得
            IEnumerable<IBubbleMono> nearbyBubbles = _bubbleCluster.Bubbles
                        .Where(b => (b.Transform.position - targetBubblePosition).sqrMagnitude <= Mathf.Pow(chainRadius, 2));

            _bubbleCombo.AddRemainingChainBubble(nearbyBubbles);

            // 周辺のBubbleを割る
            foreach (IBubbleMono nearbyBubble in nearbyBubbles)
            {
                // 0.5秒後に周辺のbubbleをdestory
                // * 遅延処理いれないとbubble同士が循環参照して(おそらく)unityが落ちるので注意
                IDisposable disposable = Observable.Timer(TimeSpan.FromSeconds(0.5f))
                    .Subscribe(_ =>
                    {
                        if (nearbyBubble == null) return;
                        // nearbyBubble.InvokeOnDead();
                    });

                // BubbleがDestoryしたら、上記の遅延処理をdisposeさせるよう設定
                IBubbleBuildSetter aroundBubbleSetter = (BubbleMono)nearbyBubble;
                // aroundBubbleSetter.OnDead += () => disposable.Dispose();

            }
        }
    }
}


#nullable enable

using UnityEngine;
using R3;
using VContainer;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Shabon.Bubble
{
    /// <summary>
    /// バブルの連鎖処理を行うクラス
    /// </summary>
    public class BubbleChain : IBubbleChain
    {
        private readonly BubbleCluster _bubbleCluster;

        [Inject]
        public BubbleChain(BubbleCluster bubbleCluster)
        {
            _bubbleCluster = bubbleCluster;
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
                        .Where(b => (b.Transform.position - targetBubblePosition).sqrMagnitude <= Mathf.Pow(chainRadius, 2))  // 半径以内
                        .Where(b => b.Transform.position.z > targetBubblePosition.z)   // ターゲットより奥にある
                        .OrderBy(b => b.Transform.position.z);  // 近い順

            foreach (var b in nearbyBubbles)
            {
                Debug.Log($"{b.Transform.position.z}");
            }

            Debug.Log($"{nearbyBubbles.Count()}");

            // 一体でもいたら
            if (nearbyBubbles.Any())
            {
                Observable.Timer(TimeSpan.FromSeconds(0.2f)).
                Subscribe(_ =>
                {
                    var bubble = nearbyBubbles.FirstOrDefault();
                    ExecuteBubbleChain(bubble, chainRadius);    // todo めっちゃ仮
                    bubble.Death.InvokeDeath(BubbleDeathType.Chain);    // 一番前を割る
                });
            }
        }
    }
}


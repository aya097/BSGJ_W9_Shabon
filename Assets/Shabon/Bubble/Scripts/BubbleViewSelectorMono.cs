#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shabon.Bubble
{
    /// <summary>
    /// BubbleTypeに応じてViewを返す
    /// </summary>
    public class BubbleViewSelectorMono : MonoBehaviour
    {
        [SerializeField] private BubbleViewMono defaultView = null!;
        [SerializeField] private List<BubbleViewEnumPair> bubbleViews = new();


        // BubbleTypeに応じてViewを返す
        public BubbleViewMono GetViewMono(BubbleType bubbleType)
        {
            // 該当のBubbleViewMonoを取得
            var bubbleView = bubbleViews.Where(b => b.Type == bubbleType).FirstOrDefault().ViewMono;

            // もしなければデフォルトを返す
            if (bubbleView == null)
            {
                defaultView.SetBubbleType(bubbleType);
                return defaultView;
            }
            bubbleView.SetBubbleType(bubbleType);
            return bubbleView;
        }
    }

    [Serializable]
    public class BubbleViewEnumPair
    {
        [SerializeField] private BubbleType type = BubbleType.None;
        [SerializeField] private BubbleViewMono viewMono = null!;

        // ゲッター
        public BubbleType Type => type;
        public BubbleViewMono ViewMono => viewMono;
    }
}
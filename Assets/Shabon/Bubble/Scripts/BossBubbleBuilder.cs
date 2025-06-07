using UnityEngine;

namespace Shabon.Bubble
{
    /// <summary>
    /// BossBubbleの個性を付与するクラス
    /// </summary>
    public class BossBubbleBuilder : IBubbleBuilder
    {
        /// <summary>
        /// 個性を付与するメソッド
        /// </summary>
        public void Build(IBubbleBuildSetter bubbleSetter, IBubbleMono bubbleMono, IBubbleData bubbleData, BubbleViewMono bubbleViewMono)
        {
            // のちのち実装
            Debug.Log("個性を付与します");

        }

    }
}

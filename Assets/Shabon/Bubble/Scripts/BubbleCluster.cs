#nullable enable

using System.Collections.Generic;

namespace Shabon.Bubble
{
    /// <summary>
    /// BubbleMonoを集約するクラス
    /// </summary>
    public class BubbleCluster
    {
        public IEnumerable<IBubbleMono> Bubbles => _bubbles;
        List<IBubbleMono> _bubbles = new();

        public void Add(IBubbleMono bubble)
        {
            _bubbles.Add(bubble);
        }

        public void Remove(IBubbleMono bubble)
        {
            _bubbles.Remove(bubble);
        }
    }
}

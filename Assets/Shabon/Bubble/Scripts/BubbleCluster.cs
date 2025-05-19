#nullable enable

using System.Collections.Generic;
using System.Diagnostics;

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
            UnityEngine.Debug.Log($"{string.Join(",", _bubbles)}");
        }

        public void Remove(IBubbleMono bubble)
        {
            _bubbles.Remove(bubble);
            UnityEngine.Debug.Log($"{string.Join(",", _bubbles)}");
        }
    }
}

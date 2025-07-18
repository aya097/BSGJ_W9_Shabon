#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace Ranking
{
    public class RankingViewSelectorMono : MonoBehaviour
    {
        public bool IsAvailable;
        [SerializeField] private RankingViewMono view0 = null!;
        [SerializeField] private RankingViewMono view1 = null!;

        private bool _current = false;

        public void SetTexts(IEnumerable<string> texts)
        {
            if (_current)
            {
                view0.SetTexts(texts);
            }
            else
            {
                view1.SetTexts(texts);
            }
            _current ^= true;
        }
    }
}
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

        private bool _current = true;

        public void SetTexts(IEnumerable<string> texts, ResultEnum resultType)
        {
            if (_current)
            {
                view0.SetTexts(texts);
                view0.SetResultType(resultType);
                view0.Open();
                view1.Close();
            }
            else
            {
                view1.SetTexts(texts);
                view1.SetResultType(resultType);
                view1.Open();
                view0.Close();
            }
            _current ^= true;
        }
    }
}
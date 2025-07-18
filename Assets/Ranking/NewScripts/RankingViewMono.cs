#nullable enable
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Ranking
{
    // リザルトデータの種類
    public enum ResultEnum
    {
        None,
        Dirt,
        Score,
        Combo,
        Clap,
        SumDirt,
        Breath,
        Calorie,
        Boss,
    }

    public class RankingViewMono : MonoBehaviour
    {
        // 表示するリザルトデータ
        [SerializeField] private ResultEnum resultType = ResultEnum.None;

        [SerializeField] private List<TMP_Text> resultTexts = new List<TMP_Text>();

        public ResultEnum ResultType => resultType;
        public int ResultCount => resultTexts.Count;

        public void SetTexts(IEnumerable<string> texts)
        {
            int index = 0;
            foreach (var text in texts)
            {
                if (index < resultTexts.Count)
                {
                    resultTexts[index].text = text;
                }
                index++;
            }
        }

    }
}
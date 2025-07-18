#nullable enable
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;


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
        [SerializeField] private LocalizeStringEvent topText = null!;

        [SerializeField] private List<TMP_Text> resultTexts = new List<TMP_Text>();
        [SerializeField] private List<LocalizeStringEvent> unitTexts = new List<LocalizeStringEvent>();

        public ResultEnum ResultType => resultType;
        public int ResultCount => resultTexts.Count;

        public void SetResultType(ResultEnum type)
        {
            resultType = type;

            var localizedString = new LocalizedString();
            localizedString.TableReference = "TextTable"; // テーブル名
            localizedString.TableEntryReference = "result_" + resultType.ToString();   // エントリ名（キー）

            topText.StringReference = localizedString;
            topText.RefreshString();

            // 各テキストの単位も更新
            foreach (var unitText in unitTexts)
            {
                var localizedStringUnit = new LocalizedString();
                localizedStringUnit.TableReference = "TextTable"; // テーブル名
                localizedStringUnit.TableEntryReference = "result_" + resultType.ToString() + "_unit";   // エントリ名（キー）

                unitText.StringReference = localizedStringUnit;
                unitText.RefreshString();
            }
        }

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
            // 残りのテキストを空にする
            for (int i = index; i < resultTexts.Count; i++)
            {
                resultTexts[i].text = "-";
            }

        }
    }
}
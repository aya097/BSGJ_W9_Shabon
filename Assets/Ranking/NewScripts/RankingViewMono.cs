#nullable enable
using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
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
        [SerializeField] private RectTransform viewObject = null!;
        [SerializeField] private LocalizeStringEvent topText = null!;

        [SerializeField] private List<TMP_Text> resultTexts = new List<TMP_Text>();
        [SerializeField] private List<LocalizeStringEvent> unitTexts = new List<LocalizeStringEvent>();

        [SerializeField] private float windowSpeed = 0.5f;
        [SerializeField] Ease ease;

        public ResultEnum _resultType;
        public bool IsAvailable => _isAvailable;
        public int ResultCount => resultTexts.Count;

        private bool _isAvailable = true;

        public void Open(bool isInverse = false)
        {
            float from = 2000f * (isInverse ? -1 : 1);
            float to = 0f;
            _isAvailable = false;
            LMotion.Create(from, to, windowSpeed)
                .WithOnComplete(() => _isAvailable = true)
                .WithEase(ease)
                .BindToAnchoredPositionX(viewObject)
                .AddTo(this);
        }

        public void Close(bool isInverse = false)
        {
            float from = 0f;
            float to = -2000f * (isInverse ? -1 : 1);
            _isAvailable = false;
            LMotion.Create(from, to, windowSpeed)
                .WithOnComplete(() => _isAvailable = true)
                .WithEase(ease)
                .BindToAnchoredPositionX(viewObject)
                .AddTo(this);
        }

        public void SetResultType(ResultEnum type)
        {
            _resultType = type;

            var localizedString = new LocalizedString();
            localizedString.TableReference = "TextTable"; // テーブル名
            localizedString.TableEntryReference = "result_" + _resultType.ToString();   // エントリ名（キー）

            topText.StringReference = localizedString;
            topText.RefreshString();

            // 各テキストの単位も更新
            foreach (var unitText in unitTexts)
            {
                var localizedStringUnit = new LocalizedString();
                localizedStringUnit.TableReference = "TextTable"; // テーブル名
                localizedStringUnit.TableEntryReference = "result_" + _resultType.ToString() + "_unit";   // エントリ名（キー）

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
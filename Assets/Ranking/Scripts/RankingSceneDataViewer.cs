using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Shabon.Game
{
    public class RankingSceneDataViewer : MonoBehaviour
    {
        [SerializeField] TMP_Text breathTimeRankText = null!;
        [SerializeField] TMP_Text bossBattleTimeRankText = null!;
        [SerializeField] TMP_Text breathStrengthSumRankText = null!;
        [SerializeField] TMP_Text dirtDecreaseCountRankText = null!;
        [SerializeField] TMP_Text comboRankText = null!;
        [SerializeField] TMP_Text dirtRankText = null!;
        [SerializeField] TMP_Text clapRankText = null!;

        void Start()
        {
            float yOffset = -1800f;
            FixTextPosition(breathTimeRankText, yOffset);
            FixTextPosition(bossBattleTimeRankText, yOffset);
            FixTextPosition(breathStrengthSumRankText, yOffset);
            FixTextPosition(dirtDecreaseCountRankText, yOffset);
            FixTextPosition(comboRankText, yOffset);
            FixTextPosition(dirtRankText, yOffset);
            FixTextPosition(clapRankText, yOffset);

            string rankingPath = Path.Combine(Application.dataPath, "Ranking/Scripts/RankingSceneData.json");
            if (File.Exists(rankingPath))
            {
                string json = File.ReadAllText(rankingPath);
                RankingSceneData rankingData = JsonUtility.FromJson<RankingSceneData>(json);

                breathTimeRankText.text = BuildRankingText(rankingData.FinalBreathTimeRanking);
                bossBattleTimeRankText.text = BuildRankingText(rankingData.BossBattleTimeRanking);
                breathStrengthSumRankText.text = BuildRankingText(rankingData.FinalBreathStrengthSumRanking);
                dirtDecreaseCountRankText.text = BuildRankingText(rankingData.FinalDirtDecreaseCountRanking);
                comboRankText.text = BuildRankingText(rankingData.FinalComboRanking);
                dirtRankText.text = BuildRankingText(rankingData.FinalDirtRanking);
                clapRankText.text = BuildRankingText(rankingData.FinalClapCountRanking);
            }
        }

        // RectTransformを上中央揃え＆Y位置を指定
        private void FixTextPosition(TMP_Text text, float yOffset)
        {
            if (text == null) return;
            var rt = text.rectTransform;
            rt.anchorMin = new Vector2(0.5f, 1f);   // 上中央
            rt.anchorMax = new Vector2(0.5f, 1f);   // 上中央
            rt.pivot = new Vector2(0.5f, 1f);       // 上中央
            rt.anchoredPosition = new Vector2(0, yOffset); // 上端中央から下にずらす
            text.alignment = TextAlignmentOptions.Center;   // 中央揃え
        }

        // タイトル行なし
        private string BuildRankingText<T>(List<T> scores)
        {
            string[] colors = { "#FFE066", "#FFFFFF", "#B87333" }; // 金・銀・銅
            int[] sizes = { 36, 36, 36 };

            var lines = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                string value = (scores != null && i < scores.Count) ? scores[i]?.ToString() ?? "---" : "---";
                lines.Add($"<color={colors[i]}><size={sizes[i]}>{i + 1}: {value}</size></color>");
            }
            return string.Join("\n", lines);
        }

        [Serializable]
        private class RankingSceneData
        {
            public List<int> FinalDirtRanking;
            public List<int> FinalComboRanking;
            public List<int> FinalClapCountRanking;
            public List<int> FinalDirtDecreaseCountRanking;
            public List<float> FinalBreathTimeRanking;
            public List<float> FinalBreathStrengthSumRanking;
            public List<float> BossBattleTimeRanking;
        }
    }
}
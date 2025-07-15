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
        [SerializeField] TMP_Text calorieRankText = null!;
        [SerializeField] TMP_Text dirtValueCountSumRankText = null!;
        [SerializeField] TMP_Text comboRankText = null!;
        [SerializeField] TMP_Text dirtRankText = null!;
        [SerializeField] TMP_Text clapRankText = null!;

        void Start()
        {
            float yOffset = -1800f;
            FixTextPosition(breathTimeRankText, yOffset);
            FixTextPosition(bossBattleTimeRankText, yOffset);
            FixTextPosition(calorieRankText, yOffset);
            FixTextPosition(dirtValueCountSumRankText, yOffset);
            FixTextPosition(comboRankText, yOffset);
            FixTextPosition(dirtRankText, yOffset);
            FixTextPosition(clapRankText, yOffset);

            string rankingPath = Path.Combine(Application.dataPath, "Ranking/Scripts/RankingSceneData.json");
            if (File.Exists(rankingPath))
            {
                string json = "";
                try
                {
                    json = File.ReadAllText(rankingPath);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"RankingSceneData.json の読み込みに失敗しました: {e}");
                    return;
                }
                RankingSceneData rankingData = null!;
                try
                {
                    rankingData = JsonUtility.FromJson<RankingSceneData>(json);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"RankingSceneData.json のパースに失敗しました: {e}");
                    return;
                }

                breathTimeRankText.text = BuildRankingText(rankingData.FinalBreathTimeRanking);
                bossBattleTimeRankText.text = BuildRankingText(rankingData.BossBattleTimeRanking);
                calorieRankText.text = BuildRankingText(rankingData.CalorieRanking, "kcal");
                dirtValueCountSumRankText.text = BuildRankingText(rankingData.DirtValueCountSumRanking, " times");
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
        private string BuildRankingText<T>(List<T> scores, string unit = "")
        {
            string[] colors = { "#FFE066", "#FFFFFF", "#B87333" }; // 金・銀・銅
            int[] sizes = { 36, 36, 36 };

            var lines = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                string value = (scores != null && i < scores.Count) ? scores[i]?.ToString() ?? "---" : "---";
                // 少数の場合は1桁まで
                if (scores != null && i < scores.Count && scores[i] is float f)
                {
                    value = f.ToString("0.0");
                }
                lines.Add($"<color={colors[i]}><size={sizes[i]}>{i + 1}: {value}{unit}</size></color>");
            }
            return string.Join("\n", lines);
        }

        [Serializable]
        private class RankingSceneData
        {
            public List<int> FinalDirtRanking;
            public List<int> FinalComboRanking;
            public List<int> FinalClapCountRanking;
            public List<int> DirtValueCountSumRanking;
            public List<float> FinalBreathTimeRanking;
            public List<float> CalorieRanking;
            public List<float> BossBattleTimeRanking;
        }
    }
}
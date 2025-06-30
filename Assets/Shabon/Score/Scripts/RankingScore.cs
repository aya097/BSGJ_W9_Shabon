#nullable enable
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Shabon.Score
{
    /// <summary>
    /// スコアをJSONファイルに保存・読み込みするクラス
    /// </summary>
    public static class RankingScore
    {
        private static readonly string FilePath = "Assets/Shabon/Score/Scripts/Ranking.json";

        /// <summary>
        /// スコアを保存するメソッド
        /// </summary>
        public static void SaveScore(int score)
        {
#if UNITY_WEBGL
            return;
#endif
            Debug.Log($"Saving Score: {score}");
            List<int> scores = LoadScores();
            scores.Add(score);
            scores.Sort((a, b) => b.CompareTo(a)); // 降順にソート

            File.WriteAllText(FilePath, JsonUtility.ToJson(new ScoreData(scores)));
        }

        /// <summary>
        /// スコアを読み込むメソッド
        /// </summary>
        public static List<int> LoadScores()
        {
#if UNITY_WEBGL
            return new List<int>();
#endif
            if (!File.Exists(FilePath))
            {
                return new List<int>();
            }

            string json = File.ReadAllText(FilePath);
            ScoreData? data = JsonUtility.FromJson<ScoreData>(json);
            return data?.Scores ?? new List<int>();
        }

        [System.Serializable]
        private class ScoreData
        {
            public List<int> Scores;

            public ScoreData(List<int> scores)
            {
                Scores = scores;
            }
        }
    }
}
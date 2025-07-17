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

        // ↓必ずStreamingAssetsに保存
        private static readonly string PersistentPath = Application.dataPath + "/StreamingAssets/Ranking.json";
        private static readonly string StreamingPath = Application.dataPath + "/StreamingAssets/Ranking.json";


        /// <summary>
        /// スコアを保存するメソッド
        /// </summary>
        public static void SaveScore(int score)
        {
            List<int> scores = LoadScores();
            scores.Add(score);
            scores.Sort((a, b) => b.CompareTo(a)); // 降順

            try
            {
                Debug.Log($"[RankingScore] Save to: {PersistentPath}");
                File.WriteAllText(PersistentPath, JsonUtility.ToJson(new ScoreData(scores)));
            }
            catch (System.Exception e)
            {
                Debug.LogError($"スコア保存時にエラー: {e}");
            }
        }

        /// <summary>
        /// スコアを読み込むメソッド
        /// </summary>
        public static List<int> LoadScores()
        {
            string path = PersistentPath;
            if (!File.Exists(path))
            {
                // persistentDataPathに無ければStreamingAssetsから初期データを読む
                path = StreamingPath;
            }

            List<int> scores = new List<int>();
            try
            {
                string json;
#if UNITY_ANDROID && !UNITY_EDITOR
                string url = "jar:file://" + path;
                using (var request = UnityEngine.Networking.UnityWebRequest.Get(url))
                {
                    var op = request.SendWebRequest();
                    while (!op.isDone) { }
                    if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
                        json = request.downloadHandler.text;
                    else
                        return scores;
                }
#else
                json = File.Exists(path) ? File.ReadAllText(path) : "";
#endif
                if (!string.IsNullOrEmpty(json))
                {
                    ScoreData? data = JsonUtility.FromJson<ScoreData>(json);
                    scores = data?.Scores ?? new List<int>();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"スコア読み込み時にエラー: {e}");
            }
            return scores;
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
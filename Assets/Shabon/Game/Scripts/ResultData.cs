using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shabon.Game
{
    [System.Serializable]
    public class ResultDataModel
    {
        public int FinalDirt;
        public int FinalScore; // ★追加
        public int FinalCombo;
        public int FinalClapCount;
        public int DirtValueCountSum;
        public float FinalBreathTime;
        public float Calorie;
        public float BossBattleTime;
    }

    [System.Serializable]
    public class ResultDataList
    {
        public List<ResultDataModel> Results = new();
    }

    public static class ResultData
    {
        public static int FinalDirt { get; set; }
        public static int FinalScore { get; set; }
        public static int FinalCombo { get; set; }
        public static int FinalClapCount { get; set; }
        public static int FinalDirtDecreaseCount { get; set; }
        public static int FinalDirtIncreaseCount { get; set; }
        public static float FinalBreathTime { get; set; }
        public static float FinalBreathStrengthSum { get; set; }
        public static float FinalCalorie { get; set; }
        public static float BossBattleTime { get; set; }

        // ↓必ずStreamingAssetsに保存
        private static readonly string PersistentPath = Application.dataPath + "/StreamingAssets/ResultData.json";
        private static readonly string StreamingPath = Application.dataPath + "/StreamingAssets/ResultData.json";

        /// <summary>
        /// データを保存するメソッド（追記方式）
        /// </summary>
        public static void SaveResults(
            int dirt, int score, int combo, int clapCount = 0,
            int dirtValueCountSum = 0,
            float breathTime = 0, float breathStrengthSum = 0,
            float bossBattleTime = 0
        )
        {
            FinalDirt = dirt;
            FinalScore = score;
            FinalCombo = combo;
            FinalClapCount = clapCount;
            FinalDirtIncreaseCount = dirtValueCountSum;
            FinalBreathTime = breathTime;
            BossBattleTime = bossBattleTime;

            // 消費カロリー計算
            float breathCalorie = (breathStrengthSum / 60f) * 2f;
            float clapCalorie = clapCount * 0.15f;
            float calorie = breathCalorie + clapCalorie;

            var model = new ResultDataModel
            {
                FinalDirt = dirt,
                FinalScore = score,
                FinalCombo = combo,
                FinalClapCount = clapCount,
                DirtValueCountSum = dirtValueCountSum,
                FinalBreathTime = breathTime,
                Calorie = calorie,
                BossBattleTime = bossBattleTime
            };

            // 既存データを読み込む
            List<ResultDataModel> results = new();
            if (File.Exists(PersistentPath))
            {
                try
                {
                    string json = File.ReadAllText(PersistentPath);
                    if (!string.IsNullOrWhiteSpace(json) && json.Trim() != "[]")
                    {
                        results = JsonUtility.FromJson<ResultDataList>("{\"Results\":" + json + "}").Results ?? new List<ResultDataModel>();
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"ResultData.jsonの読み込みに失敗: {e}");
                }
            }

            // ★直前のデータと同じ場合は保存しない
            if (results.Count > 0)
            {
                var last = results[results.Count - 1];
                if (IsSameResult(last, model))
                {
                    Debug.Log("同じリザルトデータのため保存しません");
                    return;
                }
            }

            // ★同じデータがすでに存在する場合は追加しない（全件チェック）
            if (results.Exists(r => IsSameResult(r, model)))
            {
                Debug.Log("既に同じデータが保存されているため追加しません");
                return;
            }

            results.Add(model);

            // 配列として保存
            string arrayJson = JsonUtility.ToJson(new ResultDataList { Results = results }, true);
            int start = arrayJson.IndexOf('[');
            int end = arrayJson.LastIndexOf(']');
            string onlyArray = arrayJson.Substring(start, end - start + 1);

            var dir = Path.GetDirectoryName(PersistentPath);
            if (!Directory.Exists(dir))
            {
                try { Directory.CreateDirectory(dir!); }
                catch (System.Exception e) { Debug.LogWarning($"ディレクトリ作成失敗: {e}"); }
            }

            try
            {
                File.WriteAllText(PersistentPath, onlyArray);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"ResultData.jsonの書き込みに失敗: {e}");
            }
        }

        /// <summary>
        /// 最新のデータを読み込む
        /// </summary>
        public static void LoadResults()
        {
            string path = PersistentPath;
            if (!File.Exists(path))
                path = StreamingPath;
            string json = File.Exists(path) ? File.ReadAllText(path) : "";
            if (string.IsNullOrWhiteSpace(json)) return;
            var results = JsonUtility.FromJson<ResultDataList>("{\"Results\":" + json + "}").Results;
            if (results != null && results.Count > 0)
            {
                var model = results[results.Count - 1]; // 最新
                FinalDirt = model.FinalDirt;
                FinalScore = model.FinalScore; // ★追加
                FinalCombo = model.FinalCombo;
                FinalClapCount = model.FinalClapCount;
                FinalDirtIncreaseCount = model.DirtValueCountSum;
                FinalBreathTime = model.FinalBreathTime;
                FinalBreathStrengthSum = model.Calorie; // ←ここはCalorieではなくbreathStrengthSumを保存したい場合は修正
                FinalCalorie = model.Calorie;
                BossBattleTime = model.BossBattleTime;
            }
        }

        // ★追加: データ比較用
        private static bool IsSameResult(ResultDataModel a, ResultDataModel b)
        {
            return a.FinalDirt == b.FinalDirt &&
                   a.FinalScore == b.FinalScore &&
                   a.FinalCombo == b.FinalCombo &&
                   a.FinalClapCount == b.FinalClapCount &&
                   a.DirtValueCountSum == b.DirtValueCountSum &&
                   Mathf.Approximately(a.FinalBreathTime, b.FinalBreathTime) &&
                   Mathf.Approximately(a.Calorie, b.Calorie) &&
                   Mathf.Approximately(a.BossBattleTime, b.BossBattleTime);
        }
    }
}

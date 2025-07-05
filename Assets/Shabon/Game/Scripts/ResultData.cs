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
        public int FinalScore;
        public int FinalCombo;
        public int FinalClapCount;
        public int FinalDirtDecreaseCount;
        public float FinalBreathTime;
        public float FinalBreathStrengthSum;
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
        public static float FinalBreathTime { get; set; }
        public static float FinalBreathStrengthSum { get; set; }
        public static float BossBattleTime { get; set; }

        private static readonly string FilePath = Path.Combine(Application.dataPath, "Shabon/Game/Scripts/ResultData.json");


        /// <summary>
        /// データを保存するメソッド（追記方式）
        /// </summary>
        public static void SaveResults(
            int dirt, int score, int combo, int clapCount = 0, int dirtDecreaseCount = 0,
            float breathTime = 0, float breathStrengthSum = 0,
            float bossBattleTime = 0
        )
        {
            FinalDirt = dirt;
            FinalScore = score;
            FinalCombo = combo;
            FinalClapCount = clapCount;
            FinalDirtDecreaseCount = dirtDecreaseCount;
            FinalBreathTime = breathTime;
            FinalBreathStrengthSum = breathStrengthSum;
            BossBattleTime = bossBattleTime;

            var model = new ResultDataModel
            {
                FinalDirt = dirt,
                FinalScore = score,
                FinalCombo = combo,
                FinalClapCount = clapCount,
                FinalDirtDecreaseCount = dirtDecreaseCount,
                FinalBreathTime = breathTime,
                FinalBreathStrengthSum = breathStrengthSum,
                BossBattleTime = bossBattleTime
            };

            // 既存データを読み込む
            List<ResultDataModel> results = new();
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                if (!string.IsNullOrWhiteSpace(json) && json.Trim() != "[]")
                {
                    try
                    {
                        results = JsonUtility.FromJson<ResultDataList>("{\"Results\":" + json + "}").Results;
                    }
                    catch
                    {
                        // パース失敗時は空配列
                        results = new List<ResultDataModel>();
                    }
                }
            }
            results.Add(model);

            // 配列として保存
            string arrayJson = JsonUtility.ToJson(new ResultDataList { Results = results }, true);
            // JsonUtility.ToJsonで配列部分だけ抜き出す
            int start = arrayJson.IndexOf('[');
            int end = arrayJson.LastIndexOf(']');
            string onlyArray = arrayJson.Substring(start, end - start + 1);

            // ディレクトリがなければ作成
            var dir = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.WriteAllText(FilePath, onlyArray);
        }

        /// <summary>
        /// 最新のデータを読み込む
        /// </summary>
        public static void LoadResults()
        {
            if (!File.Exists(FilePath)) return;
            string json = File.ReadAllText(FilePath);
            if (string.IsNullOrWhiteSpace(json)) return;
            var results = JsonUtility.FromJson<ResultDataList>("{\"Results\":" + json + "}").Results;
            if (results != null && results.Count > 0)
            {
                var model = results[results.Count - 1]; // 最新
                FinalDirt = model.FinalDirt;
                FinalScore = model.FinalScore;
                FinalCombo = model.FinalCombo;
                FinalClapCount = model.FinalClapCount;
                FinalDirtDecreaseCount = model.FinalDirtDecreaseCount;
                FinalBreathTime = model.FinalBreathTime;
                FinalBreathStrengthSum = model.FinalBreathStrengthSum;
                BossBattleTime = model.BossBattleTime;
            }
        }
    }
}

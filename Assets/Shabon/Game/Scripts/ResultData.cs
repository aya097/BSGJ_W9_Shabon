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
        public static float BossBattleTime { get; set; }

        private static readonly string FilePath = Path.Combine(Application.dataPath, "Shabon/Game/Scripts/ResultData.json");


        /// <summary>
        /// データを保存するメソッド（追記方式）
        /// </summary>
        public static void SaveResults(
            int dirt, int combo, int clapCount = 0,
            int dirtValueCountSum = 0,
            float breathTime = 0, float breathStrengthSum = 0,
            float bossBattleTime = 0
        )
        {
            FinalDirt = dirt;
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
                FinalCombo = combo,
                FinalClapCount = clapCount,
                DirtValueCountSum = dirtValueCountSum,
                FinalBreathTime = breathTime,
                Calorie = calorie,
                BossBattleTime = bossBattleTime
            };

            // 既存データを読み込む
            List<ResultDataModel> results = new();
            if (File.Exists(FilePath))
            {
                try
                {
                    string json = File.ReadAllText(FilePath);
                    if (!string.IsNullOrWhiteSpace(json) && json.Trim() != "[]")
                    {
                        results = JsonUtility.FromJson<ResultDataList>("{\"Results\":" + json + "}").Results;
                    }
                }
                catch (System.Exception e)
                {
                    // パース失敗時は空配列

                    Debug.LogWarning($"ファイル読み込みに失敗しました: {e}");
                    results = new List<ResultDataModel>();
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
                try
                {
                    Directory.CreateDirectory(dir);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"ディレクトリ作成に失敗しました: {e}");
                }
            }

            try
            {
                File.WriteAllText(FilePath, onlyArray);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"ファイル書き込みに失敗しました: {e}");
            }
        }

        /// <summary>
        /// 最新のデータを読み込む
        /// </summary>
        public static void LoadResults()
        {
            if (!File.Exists(FilePath)) return;
            try
            {
                string json = File.ReadAllText(FilePath);
                if (string.IsNullOrWhiteSpace(json)) return;
                var results = JsonUtility.FromJson<ResultDataList>("{\"Results\":" + json + "}").Results;
                if (results != null && results.Count > 0)
                {
                    var model = results[results.Count - 1]; // 最新
                    FinalDirt = model.FinalDirt;
                    FinalCombo = model.FinalCombo;
                    FinalClapCount = model.FinalClapCount;
                    FinalDirtIncreaseCount = model.DirtValueCountSum;
                    FinalBreathTime = model.FinalBreathTime;
                    FinalBreathStrengthSum = model.Calorie; // Calorieは合計値
                    BossBattleTime = model.BossBattleTime;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"ファイル読み込みに失敗しました: {e}");
            }
        }
    }
}

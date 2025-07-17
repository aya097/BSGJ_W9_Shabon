using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class RankingSceneDataBuilder
{
    public static void Generate(string resultPath, string rankingPath)
    {
        if (!File.Exists(resultPath))
        {
            Debug.LogError("ResultData.json が見つかりません");
            return;
        }

        string json = "";
        try
        {
            json = File.ReadAllText(resultPath);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"ResultData.json の読み込みに失敗しました: {e}");
            return;
        }

        // JSON配列としてパース
        List<ResultDataModel> results = null!;
        try
        {
            // JsonUtilityは配列直パースできないのでラップ
            results = JsonUtility.FromJson<ResultDataListWrapper>("{\"Results\":" + json + "}").Results;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"ResultData.json のパースに失敗しました: {e}");
            return;
        }
        if (results == null)
        {
            Debug.LogError("ResultData.json のパースに失敗しました");
            return;
        }

        // 各項目ごとに値が存在するものだけ抽出・ソート
        var dirtList = results.Where(r => r != null && HasField(r, "FinalDirt")).Select(r => r.FinalDirt).Distinct().OrderBy(x => x).ToList();

        var comboList = results.Where(r => r != null && HasField(r, "FinalCombo")).Select(r => r.FinalCombo).Where(x => x != 0).Distinct().OrderByDescending(x => x).ToList();
        var clapList = results.Where(r => r != null && HasField(r, "FinalClapCount")).Select(r => r.FinalClapCount).Where(x => x != 0).Distinct().OrderByDescending(x => x).ToList();
        var dirtValueCountSumList = results.Where(r => r != null && HasField(r, "DirtValueCountSum")).Select(r => r.DirtValueCountSum).Where(x => x != 0).Distinct().OrderByDescending(x => x).ToList();
        var breathTimeList = results.Where(r => r != null && HasField(r, "FinalBreathTime")).Select(r => r.FinalBreathTime).Where(x => x > 0f).Distinct().OrderByDescending(x => x).ToList();
        var calorieList = results.Where(r => r != null && HasField(r, "Calorie")).Select(r => r.Calorie).Where(x => x > 0f).Distinct().OrderByDescending(x => x).ToList();
        var bossBattleTimeList = results.Where(r => r != null && HasField(r, "BossBattleTime")).Select(r => r.BossBattleTime).Where(x => x > 0f).Distinct().OrderBy(x => x).ToList();

        var rankingData = new RankingSceneData
        {
            FinalDirtRanking = dirtList,
            FinalComboRanking = comboList,
            FinalClapCountRanking = clapList,
            DirtValueCountSumRanking = dirtValueCountSumList,
            FinalBreathTimeRanking = breathTimeList,
            CalorieRanking = calorieList,
            BossBattleTimeRanking = bossBattleTimeList
        };

        string outputJson = JsonUtility.ToJson(rankingData, true);
        try
        {
            File.WriteAllText(rankingPath, outputJson);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"RankingSceneData.json の書き込みに失敗しました: {e}");
        }
    }

    // フィールドが存在するか判定
    private static bool HasField(ResultDataModel model, string fieldName)
    {
        var field = typeof(ResultDataModel).GetField(fieldName);
        if (field == null) return false;
        // デフォルト値判定（int:0, float:0f）
        var value = field.GetValue(model);
        if (field.FieldType == typeof(int)) return true;
        if (field.FieldType == typeof(float)) return true;
        return value != null;
    }

    [System.Serializable]
    private class ResultDataModel
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
    private class ResultDataListWrapper
    {
        public List<ResultDataModel> Results;
    }

    [System.Serializable]
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
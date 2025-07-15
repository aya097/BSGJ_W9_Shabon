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

        ResultDataListWrapper results = null!;
        try
        {
            results = JsonUtility.FromJson<ResultDataListWrapper>("{\"Results\":" + json + "}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"ResultData.json のパースに失敗しました: {e}");
            return;
        }
        if (results == null || results.Results == null)
        {
            Debug.LogError("ResultData.json のパースに失敗しました");
            return;
        }

        var dirtList = results.Results.Select(r => r.FinalDirt).OrderBy(x => x).ToList();
        var comboList = results.Results.Select(r => r.FinalCombo).Where(x => x != 0).OrderByDescending(x => x).ToList();
        var clapList = results.Results.Select(r => r.FinalClapCount).Where(x => x != 0).OrderBy(x => x).ToList();
        var dirtValueCountSumList = results.Results.Select(r => r.FinalDirtDecreaseCount).Where(x => x != 0).OrderByDescending(x => x).ToList();
        var breathTimeList = results.Results.Select(r => r.FinalBreathTime).Where(x => x != 0f).OrderBy(x => x).ToList();
        var calorieList = results.Results
            .Select(r =>
            {
                float breathCalorie = (r.FinalBreathStrengthSum / 60f) * 2f;
                float clapCalorie = r.FinalClapCount * 0.15f;
                return breathCalorie + clapCalorie;
            })
            .Where(x => x != 0f)
            .OrderByDescending(x => x)
            .ToList();
        var bossBattleTimeList = results.Results.Select(r => r.BossBattleTime).Where(x => x != 0f).OrderBy(x => x).ToList();

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

    [System.Serializable]
    private class ResultDataModel
    {
        public int FinalDirt;
        public int FinalCombo;
        public int FinalClapCount;
        public int FinalDirtDecreaseCount;
        public float FinalBreathTime;
        public float FinalBreathStrengthSum;
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
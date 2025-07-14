using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RankingSceneDataGenerator
{
    [MenuItem("Tools/Generate RankingSceneData")]
    public static void GenerateRankingSceneData()
    {
        // ResultData.jsonのパスを定義
        string resultPath = Path.Combine(Application.dataPath, "Shabon/Game/Scripts/ResultData.json");
        string rankingPath = Path.Combine(Application.dataPath, "Ranking/Scripts/RankingSceneData.json");

        if (!File.Exists(resultPath))
        {
            Debug.LogError("ResultData.json が見つかりません");
            return;
        }

        string json = File.ReadAllText(resultPath);

        // JSON配列としてパース
        var results = JsonUtility.FromJson<ResultDataListWrapper>("{\"Results\":" + json + "}");
        if (results == null || results.Results == null)
        {
            Debug.LogError("ResultData.json のパースに失敗しました");
            return;
        }

        // 各項目ごとに値を抽出
        var dirtList = results.Results.Select(r => r.FinalDirt).Where(x => x != 0).OrderBy(x => x).ToList(); // 昇順
        var comboList = results.Results.Select(r => r.FinalCombo).Where(x => x != 0).OrderByDescending(x => x).ToList();
        var clapList = results.Results.Select(r => r.FinalClapCount).Where(x => x != 0).OrderBy(x => x).ToList(); // 昇順
        var dirtDecreaseList = results.Results.Select(r => r.FinalDirtDecreaseCount).Where(x => x != 0).OrderByDescending(x => x).ToList();
        var breathTimeList = results.Results.Select(r => r.FinalBreathTime).Where(x => x != 0f).OrderBy(x => x).ToList(); // 昇順
        var breathStrengthList = results.Results.Select(r => r.FinalBreathStrengthSum).Where(x => x != 0f).OrderByDescending(x => x).ToList();
        var bossBattleTimeList = results.Results.Select(r => r.BossBattleTime).Where(x => x != 0f).OrderBy(x => x).ToList(); // 昇順

        var rankingData = new RankingSceneData
        {
            FinalDirtRanking = dirtList,
            FinalComboRanking = comboList,
            FinalClapCountRanking = clapList,
            FinalDirtDecreaseCountRanking = dirtDecreaseList,
            FinalBreathTimeRanking = breathTimeList,
            FinalBreathStrengthSumRanking = breathStrengthList,
            BossBattleTimeRanking = bossBattleTimeList
        };

        string outputJson = JsonUtility.ToJson(rankingData, true);
        File.WriteAllText(rankingPath, outputJson);
        Debug.Log("RankingSceneData.json を生成しました");
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
        public List<int> FinalDirtDecreaseCountRanking;
        public List<float> FinalBreathTimeRanking;
        public List<float> FinalBreathStrengthSumRanking;
        public List<float> BossBattleTimeRanking;
    }
}
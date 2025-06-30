#nullable enable
using System;
using System.Collections.Generic;
using Shabon.Bubble;
using UnityEngine;

namespace Shabon.Param
{
    /// <summary>
    /// ゲームのルールに関するパラメータを管理するスクリプタブルオブジェクト
    /// </summary>
    [CreateAssetMenu(fileName = "GameRule", menuName = "ScriptableObjects/CreateGameRule")]
    public class GameRuleParam : ScriptableObject, IGameRuleParam
    {
        [Header("フェーズに関するパラメータ（上から1,2,3...）")]
        [SerializeField] List<GamePhaseData> gamePhaseDataList = new();

        [Header("コンボボーナスに用いるパラメータ")]
        [SerializeField] float sumBubbleScoreMultiplier;
        [SerializeField] float bubbleCountScoreMultiplier;


        // Getter
        public IEnumerable<IGamePhaseData> GetGamePhaseDataList()
        {
            return gamePhaseDataList;
        }
        public float SumBubbleScoreMultiplier
        {
            get { return sumBubbleScoreMultiplier; }
        }
        public float BubbleCountScoreMultiplier
        {
            get { return bubbleCountScoreMultiplier; }
        }

    }

    public interface IGameRuleParam
    {
        IEnumerable<IGamePhaseData> GetGamePhaseDataList();
        float SumBubbleScoreMultiplier { get; }
        float BubbleCountScoreMultiplier { get; }
    }

    /// <summary>
    /// ゲームのフェーズに関するクラス
    /// </summary>
    [Serializable]
    public class GamePhaseData : IGamePhaseData
    {
        [Header("一度に生成されるバブルの数")]
        [Min(0)]
        [SerializeField] int bubblesPerSpawn;

        [Header("バブルの生成間隔")]
        [Min(0)]
        [SerializeField] float spawnBubbleInterval;

        [Header("登場バブルと生成割合")]
        [SerializeField] List<SpawningRatio> spawningBubbles = new();

        [Header("バブルのステージ上の最大数")]
        [Min(0)]
        [SerializeField] int maxBabbleOnField;

        [Header("フェーズの実行時間")]
        [Min(0)]
        [SerializeField] float phaseLengthTime;

        [Header("このフェーズの後の休憩時間")]
        [Min(0)]
        [SerializeField] float phaseDelayTime;

        // Getter
        public int BubblesPerSpawn
        {
            get { return bubblesPerSpawn; }
        }
        public float SpawnBubbleInterval
        {
            get { return spawnBubbleInterval; }
        }
        public IEnumerable<SpawningRatio> SpawningBubbles
        {
            get { return spawningBubbles; }
        }

        public int MaxBabbleOnField
        {
            get { return maxBabbleOnField; }
        }
        public float PhaseLengthTime
        {
            get { return phaseLengthTime; }
        }
        public float PhaseDelayTime
        {
            get { return phaseDelayTime; }
        }

    }

    public interface IGamePhaseData
    {
        int BubblesPerSpawn { get; }    // 一度のバブルのスポーン数
        float SpawnBubbleInterval { get; }  // バブルの生成間隔
        IEnumerable<SpawningRatio> SpawningBubbles { get; } // 登場するバブルと生成割合

        int MaxBabbleOnField { get; }   // バブルの最大数
        float PhaseLengthTime { get; }  // フェーズの長さ
        float PhaseDelayTime { get; }   // このフェーズが終わったあとの時間
    }
    // バブルの生成確率を記載するクラス
    [Serializable]
    public class SpawningRatio
    {
        [SerializeField] private BubbleType type;
        [SerializeField] private float ratio;   // 全体に対する割あり（0~1でなくてよい）

        public BubbleType Type => type;
        public float Ratio => ratio;

    }
}
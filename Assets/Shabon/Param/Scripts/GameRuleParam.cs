#nullable enable
using System;
using System.Collections.Generic;
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

        [Header("バブルのステージ上の最大数")]
        [Min(0)]
        [SerializeField] int maxBabbleOnField;

        // Getter
        public int BubblesPerSpawn
        {
            get { return bubblesPerSpawn; }
        }
        public float SpawnBubbleInterval
        {
            get { return spawnBubbleInterval; }
        }
        public int MaxBabbleOnField
        {
            get { return maxBabbleOnField; }
        }

    }

    public interface IGamePhaseData
    {
        int BubblesPerSpawn { get; }
        float SpawnBubbleInterval { get; }
        int MaxBabbleOnField { get; }
    }
}
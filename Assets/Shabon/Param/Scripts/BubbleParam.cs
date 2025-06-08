using UnityEngine;
using System.Collections.Generic;
using System;
using Shabon.Param;

namespace Shabon.Bubble
{
    /// <summary>
    /// バブルに関するパラメータを設定するクラス
    /// </summary>
    [CreateAssetMenu(fileName = "BubbleParam", menuName = "Scriptable Objects/BubbleParam")]
    public class BubbleParam : ScriptableObject, IBubbleParam
    {
        [Header("Bubbleのパラーメータ設定")]
        [SerializeField] List<BubbleData> bubbleDataList;

        // Getter
        public IEnumerable<IBubbleData> GetBubbleDataList()
        {
            return bubbleDataList;
        }

        private void OnValidate()
        {
            foreach (var data in bubbleDataList)
            {
                if (data.IncreasingDirtValue <= 0)
                {
                    data.SetIncreasingDirtValue(1);
                }
            }
        }
    }

    public interface IBubbleParam
    {
        IEnumerable<IBubbleData> GetBubbleDataList();
    }

    /// <summary>
    /// バブルの個別のパラメータを設定するクラス
    /// </summary>
    [Serializable]
    public class BubbleData : IBubbleData
    {
        [Header("バブルの種類")]
        [SerializeField] BubbleType bubbleType;

        [Header("バブルのPrefab")]
        [SerializeField] BubbleMono bubblePrefab;

        [Header("バブルのAnimator")]
        [SerializeField] RuntimeAnimatorController bubbleAnimatorController;

        [Header("以下基本パラメータ")]

        [Header("バブルの生成エリア")]
        [SerializeField] BubbleSpawnedAreaType bubbleSpawnedArea;

        [Header("前方への移動速度(m/s)")]
        [Min(0)]
        [SerializeField] float forwardVelocity;

        [Header("ゾーン待機時間(s)")]
        [Min(0)]
        [SerializeField] float zoneWaitingTime;

        [Header("汚れレベル上昇度")]
        [Min(0)]
        [SerializeField] int increasingDirtValue;

        [Header("連鎖可能なバブル中心からの距離")]
        [Min(0)]
        [SerializeField] float chainRadius;

        [Header("バブルのスコア")]
        [SerializeField] int bubbleScore;

        // Getter
        public BubbleType BubbleType
        {
            get { return bubbleType; }
        }
        public BubbleMono BubblePrefab
        {
            get { return bubblePrefab; }
        }
        public RuntimeAnimatorController BubbleAnimatorController
        {
            get { return bubbleAnimatorController; }
        }
        public BubbleSpawnedAreaType BubbleSpawnedArea
        {
            get { return bubbleSpawnedArea; }
        }
        public float ForwardVelocity
        {
            get { return forwardVelocity; }
        }
        public float ZoneWaitingTime
        {
            get { return zoneWaitingTime; }
        }
        public int IncreasingDirtValue
        {
            get { return increasingDirtValue; }
        }
        public float ChainRadius
        {
            get { return chainRadius; }
        }
        public int BubbleScore
        {
            get { return bubbleScore; }
        }

        // Setter
        public void SetIncreasingDirtValue(int value)
        {
            increasingDirtValue = value;
        }

        
    }

    public interface IBubbleData
    {
        BubbleType BubbleType { get; }
        BubbleMono BubblePrefab { get; }
        RuntimeAnimatorController BubbleAnimatorController { get; }
        BubbleSpawnedAreaType BubbleSpawnedArea { get; }
        float ForwardVelocity { get; }
        float ZoneWaitingTime { get; }
        int IncreasingDirtValue { get; }
        float ChainRadius { get; }
        int BubbleScore { get; }
    }
}


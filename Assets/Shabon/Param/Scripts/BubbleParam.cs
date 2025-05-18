using UnityEngine;
using System.Collections.Generic;
using System;

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


        [Header("以下基本パラメータ")]

        [Header("バブルの生成位置")]
        [SerializeField] Vector3 initBubblePosition;

        [Header("前方への移動速度(m/s)")]
        [Min(0)]
        [SerializeField] float forwardVelocity;

        [Header("ゾーン待機時間(s)")]
        [Min(0)]
        [SerializeField] float zoneWaitingTime;

        [Header("汚れレベル上昇度")]
        [Min(0)]
        [SerializeField] int increasingDirtValue;

        // Getter
        public BubbleType BubbleType
        {
            get { return bubbleType; }
        }
        public BubbleMono BubblePrefab
        {
            get { return bubblePrefab; }
        }
        public Vector3 InitBubblePosition
        {
            get { return initBubblePosition; }
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
    }

    public interface IBubbleData
    {
        BubbleType BubbleType { get; }
        BubbleMono BubblePrefab { get; }
        Vector3 InitBubblePosition { get; }
        float ForwardVelocity { get; }
        float ZoneWaitingTime { get; }
        int IncreasingDirtValue { get; }
    }
}


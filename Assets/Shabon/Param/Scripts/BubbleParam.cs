using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UIElements;
using System.Drawing;

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

        [Header("バブルの生成エリア")]
        [SerializeField] BoxCollider spawnedBubbleArea;

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
        public BoxArea SpawnedBubbleArea
        {
            get
            {
                // Colliderの座標とサイズ
                return new BoxArea(spawnedBubbleArea.gameObject.transform.position, spawnedBubbleArea.size);
            }
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
        BoxArea SpawnedBubbleArea { get; }
        float ForwardVelocity { get; }
        float ZoneWaitingTime { get; }
        int IncreasingDirtValue { get; }
    }

    /// <summary>
    /// ボックス状の範囲を表す
    /// </summary>
    public struct BoxArea
    {
        public Vector3 Position { get; }    // 中心位置
        public Vector3 Size { get; }

        public BoxArea(Vector3 position, Vector3 size)
        {
            Position = position;
            Size = size;
        }
    }
}


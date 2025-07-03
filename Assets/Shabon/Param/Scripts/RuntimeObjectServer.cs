#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Shabon.Bubble;
using UnityEngine;

namespace Shabon.Param
{
    /// <summary>
    /// Unityのシーン状のオブジェクトをparamとして提供するクラス
    /// </summary>
    public class RuntimeObjectServer : MonoBehaviour, IBubbleSpawnedArea, IComboParent, IPlayerTransform
    {
        [Header("バブルのスポーンする場所を指定")]
        [Header("Typeが重複する場合はランダム")]
        [SerializeField] private List<BubbleSpawnedAreaPair> bubbleSpawnedAreas = new();

        [Header("コンボを生成する場所を指定")]
        [Header("UICanvasの下である必要がある")]
        [SerializeField] private Transform comboParent = null!;
        [Header("コンボの生成エリア")]
        [SerializeField] private RectTransform comboArea = null!;

        [Header("プレイヤーを参照")]
        [SerializeField] private Transform playerTransform = null!;

        [Header("汚れエフェクトを生成するCanvas")]
        [SerializeField] private Canvas dirtEffectCanvas = null!;


        // interface
        public BoxArea GetArea(BubbleSpawnedAreaType areaType)
        {
            var areas = bubbleSpawnedAreas.Where(b => b.AreaType == areaType);

            if (!areas.Any())
            {
                Debug.LogWarning("未設定のBubbleSpawnedAreaです");
                return new(Vector3.zero, Vector3.zero);
            }
            // ランダムに選択
            var area = areas.ElementAt(UnityEngine.Random.Range(0, areas.Count())).Area;

            return new BoxArea(area.gameObject.transform.position, area.size);
        }

        public Transform ComboParent
        {
            get { return comboParent; }
        }
        public RectTransform ComboArea
        {
            get { return comboArea; }
        }
        public Canvas DirtEffectCanvas
        {
            get { return dirtEffectCanvas; }
        }

        // プレイヤーを取得するプロパティ
        public Transform PlayerTransform
        {
            get { return playerTransform; }
        }
    }

    // SpawnedAreaに関して
    public enum BubbleSpawnedAreaType
    {
        Area0,
        Area1,
        Area2,
    };

    [Serializable]
    class BubbleSpawnedAreaPair
    {
        public BubbleSpawnedAreaType AreaType = BubbleSpawnedAreaType.Area0;
        public BoxCollider Area = null!;
    }
    public interface IBubbleSpawnedArea
    {
        BoxArea GetArea(BubbleSpawnedAreaType areaType);
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

    public interface IComboParent
    {
        Transform ComboParent { get; }
        RectTransform ComboArea { get; }
    }

    public interface IPlayerTransform
    {
        Transform PlayerTransform { get; }
    }

    public interface IDirtEffectCanvas
    {
        Canvas DirtEffectCanvas { get; }
    }
}


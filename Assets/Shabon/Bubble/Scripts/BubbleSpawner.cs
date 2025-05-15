using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shabon.Bubble
{
    /// <summary>
    /// バブルを生成を管理するクラス
    /// </summary>
    /// 
    /// バブルの種類が増えたときの変更に優しくないかも...要修正
    public class BubbleSpawner: MonoBehaviour, IBubbleSpawner
    {
        [SerializeField]
        List<BubbleMono> bubblePrefabs;

        Dictionary<BubbleType, BubbleMono> _bubblePrefabDict;

        private void Awake()
        {
            // バブルのPrefabをDictionaryに格納
            _bubblePrefabDict = new Dictionary<BubbleType, BubbleMono>();
            foreach (var bubblePrefab in bubblePrefabs)
            {
                // stringをBubbleTypeに変換
                BubbleType bubbleType = (BubbleType)Enum.Parse(typeof(BubbleType),bubblePrefab.tag);
                if (!_bubblePrefabDict.ContainsKey(bubbleType))
                {
                    _bubblePrefabDict.Add(bubbleType, bubblePrefab);
                }
            }
        }

        public void Spawn(BubbleType bubbleType)
        {
            if (!_bubblePrefabDict.TryGetValue(bubbleType, out BubbleMono bubblePrefab) || bubblePrefab == null)
            {
                Debug.LogError($"bubblePrefabDictに指定したKeyが存在しない: {bubbleType}");
                return;
            }

            // バブルの生成位置を決定
            Vector3 bubbleSpawnPos = GetSpawnPosition(bubbleType);

            // バブルを生成
            IBubbleMono bubble = Instantiate(bubblePrefab, bubbleSpawnPos, Quaternion.identity);

            // バブルビルダーを取得
            IBubbleBuilder bubbleBuilder = GetBubbleBuilder(bubbleType);
            bubbleBuilder.Build(bubble);
        }

        // バブルタイプごとの生成位置を返す
        private Vector3 GetSpawnPosition(BubbleType bubbleType)
        {
            return bubbleType switch
            {
                BubbleType.Normal => new Vector3(0, 0, 0),
                // バブルの種類が増えたらここに追加

            };
        }

        // バブルタイプごとのビルダーを返す
        private IBubbleBuilder GetBubbleBuilder(BubbleType bubbleType)
        {
            return bubbleType switch
            {
                BubbleType.Normal => new NormalBubbleBuilder(),
                // バブルの種類が増えたらここに追加

            };
        }
    }
}

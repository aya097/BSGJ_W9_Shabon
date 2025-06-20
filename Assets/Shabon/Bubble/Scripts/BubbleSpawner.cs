#nullable enable

using System.Linq;
using Shabon.Param;
using UnityEngine;
using VContainer;

namespace Shabon.Bubble
{
    /// <summary>
    /// バブルを生成するクラス
    /// </summary>
    /// 
    public class BubbleSpawner : IBubbleSpawner
    {
        private readonly IBubbleParam _bubbleParam;
        private readonly BubbleCluster _bubbleCluster;
        private readonly IBubbleSpawnedArea _bubbleSpawnedArea;
        private readonly IObjectResolver _objectResolver;   // IBubbleBuilderをnewするために使用する

        [Inject]
        public BubbleSpawner(
            IBubbleParam bubbleParam,
            BubbleCluster bubbleCluster,
            IBubbleSpawnedArea bubbleSpawnedArea,
            IObjectResolver objectResolver)
        {
            _bubbleParam = bubbleParam;
            _bubbleCluster = bubbleCluster;
            _bubbleSpawnedArea = bubbleSpawnedArea;
            _objectResolver = objectResolver;
        }

        /// <summary>
        /// BubbleType別に生成するメソッド
        /// </summary>
        public void Spawn(BubbleType bubbleType)
        {
            // bubbleType別にdataを取得する
            IBubbleData bubbleData = _bubbleParam.GetBubbleDataList().Where(b => b.BubbleType == bubbleType).FirstOrDefault();


            if (bubbleData is null)

                if (bubbleData == null)

                {
                    Debug.LogWarning("BubbleDataBaseに対象のbubbleTypeが存在しません");
                    return;
                }



            // バブルビルダーを取得
            IBubbleBuilder bubbleBuilder = GetBubbleBuilder(bubbleType);

            // スポーン位置を作成
            Vector3 spawningPosition = DecideSpawningPosition(_bubbleSpawnedArea.GetArea(bubbleData.BubbleSpawnedArea));

            // バブルを生成
            BubbleMono bubbleMono = GameObject.Instantiate(bubbleData.BubblePrefab, spawningPosition, Quaternion.identity);
            BubbleViewMono bubbleViewMono = bubbleMono.gameObject.GetComponentInChildren<BubbleViewMono>();

            // ビルド
            bubbleBuilder.Build(bubbleMono, bubbleMono, bubbleData, bubbleViewMono);

            // Clusterに登録
            _bubbleCluster.Add(bubbleMono);
        }

        /// <summary>
        /// BoxAreaからランダムに座標を作成する
        /// </summary>
        /// <param name="boxArea"></param>
        private Vector3 DecideSpawningPosition(BoxArea boxArea)
        {
            Vector3 rand = new Vector3(Random.value, Random.value, Random.value);
            // randの範囲を-0.5~0.5に
            rand = rand - Vector3.one * 0.5f;

            // 座標 + 範囲 * (-1~1)
            return boxArea.Position + Vector3.Scale(boxArea.Size, rand);
        }


        /// <summary>
        /// BubbleType別にBuilderを取得
        /// </summary>
        /// memo: Builder分けずに１つにまとめた方がスマートかも...?(ひとまずこれで)
        /// memo: あまり良くない実装かも...すみません
        private IBubbleBuilder GetBubbleBuilder(BubbleType bubbleType)
        {
            return bubbleType switch
            {
                BubbleType.Normal => _objectResolver.Resolve<NormalBubbleBuilder>(),
                BubbleType.Boss => _objectResolver.Resolve<BossBubbleBuilder>(),
                BubbleType.Quick => _objectResolver.Resolve<NormalBubbleBuilder>(),
                _ => throw new System.ArgumentOutOfRangeException(nameof(bubbleType), bubbleType, "未対応のBubbleTypeです") // defaultの処理
            };
        }
    }
}

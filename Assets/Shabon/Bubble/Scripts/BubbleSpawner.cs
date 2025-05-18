#nullable enable

using System.Linq;
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
        readonly IBubbleParam _bubbleParam = null!;

        [Inject]
        public BubbleSpawner(IBubbleParam bubbleParam)
        {
            _bubbleParam = bubbleParam;
        }

        /// <summary>
        /// BubbleType別に生成するメソッド
        /// </summary>
        public void Spawn(BubbleType bubbleType)
        {
            // bubbleType別にdataを取得する
            IBubbleData bubbleData = _bubbleParam.GetBubbleDataList().Where(b => b.BubbleType == bubbleType).FirstOrDefault();
            if (bubbleData is null)
            {
                Debug.LogWarning("BubbleDataBaseに対象のbubbleTypeが存在しません");
                return;
            }

            // バブルビルダーを取得
            IBubbleBuilder bubbleBuilder = GetBubbleBuilder(bubbleType);

            // バブルを生成
            BubbleMono bubbleMono = GameObject.Instantiate(bubbleData.BubblePrefab, bubbleData.InitBubblePosition, Quaternion.identity);

            // ビルド
            bubbleBuilder.Build(bubbleMono);
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
                BubbleType.Normal => new NormalBubbleBuilder(),
                BubbleType.Boss => new BossBubbleBuilder(),
                // バブルの種類が増えたらここに追加

                _ => throw new System.ArgumentOutOfRangeException(nameof(bubbleType), bubbleType, "未対応のBubbleTypeです") // defaultの処理

            };
        }
    }
}

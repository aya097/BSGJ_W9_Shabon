#nullable enable

using UnityEngine;
using VContainer;
using BubbleData = Shabon.Bubble.BubbleDataBase.BubbleData;

namespace Shabon.Bubble
{
    /// <summary>
    /// バブルを生成するクラス
    /// </summary>
    /// 
    /// memo: 44行目でInstanciateするのにMonoBehaviour継承せざるを得ない感じです。
    public class BubbleSpawner : MonoBehaviour, IBubbleSpawner
    {
        private BubbleDataBase _bubbleDataBase = new BubbleDataBase();


        GameObject _bubblePrefab;
        Vector3 _initBubblePosition;

        // ------------------------- テスト用 -------------------------
        //
        // [SerializeField]
        // private BubbleDataBase _bubbleDataBase;

        // void Update()
        // {
        //    // スペースキーが押されたとき
        //     if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        //     {
        //         Spawn(BubbleType.Normal); // 例：Normalバブルを生成
        //     }
        // }
        //
        // -------------------------------------------------------------

        [Inject]
        public void Construct(BubbleDataBase bubbleDataBase)
        {
            _bubbleDataBase = bubbleDataBase;
        }

        /// <summary>
        /// BubbleType別に生成するメソッド
        /// </summary>
        public void Spawn(BubbleType bubbleType)
        {
            // bubbleType別にdataを取得する
            BubbleData bubbleData = _bubbleDataBase.bubbleData.Find(x => x.bubbleType == bubbleType);
            if (bubbleData is null)
            {
                Debug.LogWarning("BubbleDataBaseに対象のbubbleTypeが存在しません");
                return;
            }
            SetBubbleDataInfo(bubbleData);

            // バブルビルダーを取得
            IBubbleBuilder bubbleBuilder = GetBubbleBuilder(bubbleType);

            // バブルを生成
            GameObject bubble = Instantiate(_bubblePrefab, _initBubblePosition, Quaternion.identity);
            IBubbleMono bubbleMono = bubble.GetComponent<BubbleMono>();
            bubbleBuilder.Build(bubbleMono);
        }

        /// <summary>
        /// BubbleDataを内部の変数にセットするメソッド
        /// </summary>
        private void SetBubbleDataInfo(BubbleData bubbleData)
        {
            _bubblePrefab = bubbleData.bubblePrefab;
            _initBubblePosition = bubbleData.initBubblePosition;
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
                BubbleType.Boss => new BossBubbleBuilder()
                // バブルの種類が増えたらここに追加

            };
        }
    }
}

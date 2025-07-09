#nullable enable

namespace Shabon.Tutorial
{
    /// <summary>
    /// このインタフェースを実行しながらチュートリアルを進める
    /// </summary>
    public interface ITutorialContext
    {
        // 最初に呼び出す
        void OnStart();
        // マイフレーム呼び出す
        void Update();
        // 最後に呼び出す
        void OnComplete();
        // 終了していいか
        bool IsFinish { get; }
    }

    // ==========以下具体的な実装==========
    public class FirstSpawn : ITutorialContext
    {
        public bool IsFinish { get; private set; }

        private readonly TutorialBubbleSpawner _tutorialBubbleSpawner = null!;

        public FirstSpawn(TutorialBubbleSpawner tutorialBubbleSpawner)
        {
            _tutorialBubbleSpawner = tutorialBubbleSpawner;
        }
        public void OnStart()
        {
            // bubbleをスポーン
            _tutorialBubbleSpawner.Spawn(Bubble.BubbleType.Normal, Param.BubbleSpawnedAreaType.Tutorial0);
        }
        public void Update()
        {

        }
        public void OnComplete()
        {

        }
    }

}
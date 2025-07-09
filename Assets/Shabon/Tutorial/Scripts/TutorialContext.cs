#nullable enable

using System;
using System.Linq;
using R3;
using Shabon.Bubble;
using Shabon.Input;
using UnityEngine;

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
        private readonly BubbleCluster _bubbleCluster = null!;
        private readonly IInputManager _inputManager = null!;
        private bool _bubbleAttacked = false;

        public FirstSpawn(TutorialBubbleSpawner tutorialBubbleSpawner,
            BubbleCluster bubbleCluster,
            IInputManager inputManager)
        {
            _tutorialBubbleSpawner = tutorialBubbleSpawner;
            _bubbleCluster = bubbleCluster;
            _inputManager = inputManager;
        }
        public void OnStart()
        {
            // bubbleをスポーン
            _tutorialBubbleSpawner.Spawn(Bubble.BubbleType.Normal, Param.BubbleSpawnedAreaType.Tutorial0);

            Observable.Timer(TimeSpan.FromSeconds(2f))
                .Subscribe(_ =>
                {
                    Debug.Log("Tutorial: バブルがやってきたよ！");
                });


            // 一定時間後に自爆
            Observable.Timer(TimeSpan.FromSeconds(6f))
                .Subscribe(_ =>
                {
                    _bubbleCluster.Bubbles.ElementAt(0).InvokeOnReach();
                });

        }
        public void Update()
        {
            // バブルが初めて攻撃したら
            if (_bubbleCluster.Bubbles.Count() == 0 && !_bubbleAttacked)
            {
                Debug.Log("Tutorial: うわあ！屋敷が汚れちゃった！！");
            }

            // 攻撃後にClapすれば
            if (_inputManager.GetClap())
            {
                IsFinish = true;
            }
        }
        public void OnComplete()
        {

        }
    }

}
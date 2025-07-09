#nullable enable

using System;
using System.Linq;
using R3;
using Shabon.Breath;
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
                _bubbleAttacked = true;
                Debug.Log("Tutorial: うわあ！屋敷が汚れちゃった！！");
            }

            // 攻撃後にClapすれば
            if (_inputManager.GetClap() && _bubbleAttacked)
            {
                IsFinish = true;
            }
        }
        public void OnComplete()
        {

        }
    }

    public class BreathSecondSpawn : ITutorialContext
    {
        public bool IsFinish { get; private set; }

        private readonly TutorialBubbleSpawner _tutorialBubbleSpawner = null!;
        private readonly BubbleCluster _bubbleCluster = null!;
        private readonly IInputManager _inputManager = null!;
        private readonly BreathModel _breathModel = null!;
        private readonly BreathGetterViewMono _breathGetterViewMono = null!;
        private bool _ableBreath = false;

        public BreathSecondSpawn(TutorialBubbleSpawner tutorialBubbleSpawner,
            BubbleCluster bubbleCluster,
            IInputManager inputManager,
            BreathModel breathModel,
            BreathGetterViewMono breathGetterViewMono)
        {
            _tutorialBubbleSpawner = tutorialBubbleSpawner;
            _bubbleCluster = bubbleCluster;
            _inputManager = inputManager;
            _breathModel = breathModel;
            _breathGetterViewMono = breathGetterViewMono;
        }
        public void OnStart()
        {
            // bubbleをスポーン
            _tutorialBubbleSpawner.Spawn(Bubble.BubbleType.Normal, Param.BubbleSpawnedAreaType.Tutorial0);

            Observable.Timer(TimeSpan.FromSeconds(2f))
                .Subscribe(_ =>
                {
                    Debug.Log("Tutorial: また来た！");
                });


            // 一定時間後に停止
            Observable.Timer(TimeSpan.FromSeconds(7f))
                .Subscribe(_ =>
                {
                    _bubbleCluster.Bubbles.ElementAt(0).Stop();
                    Debug.Log("Tutorial: 今度は息を吹いて遠ざけて！");

                });
            Observable.Timer(TimeSpan.FromSeconds(8f))
                .Subscribe(_ =>
                {
                    _ableBreath = true;
                });

        }
        public void Update()
        {
            // Breathできるようになったら
            if (_ableBreath)
            {
                // Breath
                float _ratio = _inputManager.GetHorizontalDirection();
                float amount = _inputManager.GetBreath();
                _ratio = Mathf.Max(-1, _ratio);
                _ratio = Mathf.Min(1, _ratio);
                // 向きを変える
                _breathModel.SetDirection(_ratio);
                // バブルに向いていなければ
                if (_breathGetterViewMono.GetBubbleMonos().Count() == 0)
                {
                    Debug.Log("Tutorial: バブルに向けて！");
                }
                else
                {
                    _breathModel.ApplyBreath(amount);
                    Debug.Log("Tutorial: 今だ！息を吹いて！！");
                }

                // 十分後ろに追いやれば
                if (_bubbleCluster.Bubbles.ElementAt(0).Transform.position.z > 0.9f)
                {
                    _ableBreath = false;
                    Debug.Log("Tutorial: いいね！");
                    IsFinish = true;
                }
            }
        }
        public void OnComplete()
        {

        }
    }

}
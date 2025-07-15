#nullable enable

using System;
using System.Linq;
using R3;
using Shabon.Breath;
using Shabon.Bubble;
using Shabon.Clap;
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
        private TutorialViewMono _tutorialViewMono = null!;

        public FirstSpawn(
            TutorialViewMono tutorialViewMono,
            TutorialBubbleSpawner tutorialBubbleSpawner,
            BubbleCluster bubbleCluster,
            IInputManager inputManager)
        {
            _tutorialViewMono = tutorialViewMono;
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
                    _tutorialViewMono.SetText(TutorialText.bubble_coming0);
                    Debug.Log("Tutorial: バブルがやってきたよ！");
                });


            // 一定時間後に自爆
            Observable.Timer(TimeSpan.FromSeconds(9.4f))
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
                _tutorialViewMono.SetText(TutorialText.dirty_mansion);
                Debug.Log("Tutorial: うわあ！屋敷が汚れちゃった！！");
                IsFinish = true;
            }

            // // 攻撃後にClapすれば
            // if (_inputManager.GetClap() && _bubbleAttacked)
            // {
            //     IsFinish = true;
            // }
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
        private TutorialViewMono _tutorialViewMono = null!;

        private bool _ableBreath = false;

        public BreathSecondSpawn(
            TutorialViewMono tutorialViewMono,
            TutorialBubbleSpawner tutorialBubbleSpawner,
            BubbleCluster bubbleCluster,
            IInputManager inputManager,
            BreathModel breathModel,
            BreathGetterViewMono breathGetterViewMono)
        {
            _tutorialViewMono = tutorialViewMono;
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
                    _tutorialViewMono.SetText(TutorialText.bubble_coming1);
                });


            // 一定時間後に停止
            Observable.Timer(TimeSpan.FromSeconds(7f))
                .Subscribe(_ =>
                {
                    _bubbleCluster.Bubbles.ElementAt(0).Stop();
                    _tutorialViewMono.SetText(TutorialText.blow_air);
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
                    _tutorialViewMono.SetText(TutorialText.to_bubble);
                    Debug.Log("Tutorial: バブルに向けて！");
                }
                else
                {
                    _breathModel.ApplyBreath(amount);
                    _tutorialViewMono.SetText(TutorialText.now_blow);
                    Debug.Log("Tutorial: 今だ！息を吹いて！！");
                }

                // 十分後ろに追いやれば
                if (_bubbleCluster.Bubbles.ElementAt(0).Transform.position.z > 0.9f)
                {
                    _ableBreath = false;
                    _tutorialViewMono.SetText(TutorialText.nice_blow);
                    Debug.Log("Tutorial: いいね！");
                    _breathModel.ApplyBreath(0);
                    IsFinish = true;
                }
            }
        }
        public void OnComplete()
        {

        }
    }

    public class ClapThirdSpawn : ITutorialContext
    {
        public bool IsFinish { get; private set; }

        private readonly TutorialBubbleSpawner _tutorialBubbleSpawner = null!;
        private readonly BubbleCluster _bubbleCluster = null!;
        private readonly IInputManager _inputManager = null!;
        private readonly ClapModel _clapModel = null!;
        private TutorialViewMono _tutorialViewMono = null!;

        private bool _ableClap = false;

        public ClapThirdSpawn(
            TutorialViewMono tutorialViewMono,
            TutorialBubbleSpawner tutorialBubbleSpawner,
            BubbleCluster bubbleCluster,
            IInputManager inputManager,
            ClapModel clapModel)
        {
            _tutorialViewMono = tutorialViewMono;
            _tutorialBubbleSpawner = tutorialBubbleSpawner;
            _bubbleCluster = bubbleCluster;
            _inputManager = inputManager;
            _clapModel = clapModel;
        }
        public void OnStart()
        {
            // bubbleをスポーン
            _tutorialBubbleSpawner.Spawn(Bubble.BubbleType.Normal, Param.BubbleSpawnedAreaType.Tutorial1);

            Observable.Timer(TimeSpan.FromSeconds(2f))
                .Subscribe(_ =>
                {
                    _tutorialViewMono.SetText(TutorialText.bubble_coming2);
                    Debug.Log("Tutorial: またバブルがやってきたよ！");
                });


            // 一定時間後にさっきのバブルも動かす
            Observable.Timer(TimeSpan.FromSeconds(2f))
                .Subscribe(_ =>
                {
                    _bubbleCluster.Bubbles.ElementAt(0).Resume();
                    _tutorialViewMono.SetText(TutorialText.lure_bubble);
                    Debug.Log("Tutorial: 引きつけて一気に倒してやろう！！");
                });
            Observable.Timer(TimeSpan.FromSeconds(5f))
                .Subscribe(_ =>
                {
                    _tutorialViewMono.SetText(TutorialText.prepare_hand);
                    Debug.Log("Tutorial: 手をかまえて！！！");
                });
            // いい感じの位置で停止
            Observable.Timer(TimeSpan.FromSeconds(9.5f))
                .Subscribe(_ =>
                {
                    _ableClap = true;
                    _bubbleCluster.Bubbles.ElementAt(0).Stop();
                    _bubbleCluster.Bubbles.ElementAt(1).Stop();
                    _tutorialViewMono.SetText(TutorialText.now_clap);
                    Debug.Log("Tutorial: 今だ！！手を叩いて！！");
                });

        }
        public void Update()
        {
            if (_ableClap)
            {
                // Clap
                if (_inputManager.GetClap())
                {
                    _ableClap = false;
                    _clapModel.ApplyClap(1f);
                    Observable.Timer(TimeSpan.FromSeconds(2f))
                        .Subscribe(_ =>
                        {
                            _tutorialViewMono.SetText(TutorialText.clean_mansion);
                            Debug.Log("Tutorial: やった！倒せたぞ！屋敷もキレイになったね！");
                        });
                    Observable.Timer(TimeSpan.FromSeconds(5f))
                        .Subscribe(_ =>
                        {
                            _tutorialViewMono.SetText(TutorialText.thanks);
                            Debug.Log("Tutorial: この調子でがんばりたまえ！ハッハッハッ！");
                            IsFinish = true;
                        });
                }
            }

        }
        public void OnComplete()
        {

        }
    }

}
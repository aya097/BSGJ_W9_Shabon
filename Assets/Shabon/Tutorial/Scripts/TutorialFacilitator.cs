#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Shabon.Breath;
using Shabon.Bubble;
using Shabon.Clap;
using Shabon.Input;
using VContainer;
using VContainer.Unity;

namespace Shabon.Tutorial
{
    /// <summary>
    /// チュートリアルを進行するクラス。
    /// </summary>
    public class TutorialFacilitator : ITickable
    {
        // チュートリアルを順番に入れるリスト(後戻りもできるようにList) 
        private readonly List<ITutorialContext> tutorialContexts = new();

        // 現在のチュートリアル進行状況
        private int _currentTutorialIndex = 0;

        // チュートリアル中
        private bool _isInTutorial = false;

        // チュートリアルが終了したときに呼ぶ
        private Action? _onComplete;


        [Inject]
        public TutorialFacilitator(TutorialBubbleSpawner bubbleSpawner,
            BubbleCluster bubbleCluster,
            IInputManager inputManager,
            BreathModel breathModel,
            BreathGetterViewMono breathGetterViewMono,
            ClapModel clapModel,
            TutorialViewMono tutorialViewMono)
        {

            _isInTutorial = false;

            // チュートリアル生成
            tutorialContexts.Add(new FirstSpawn(tutorialViewMono, bubbleSpawner, bubbleCluster, inputManager));
            tutorialContexts.Add(new BreathSecondSpawn(tutorialViewMono, bubbleSpawner, bubbleCluster, inputManager, breathModel, breathGetterViewMono));
            tutorialContexts.Add(new ClapThirdSpawn(tutorialViewMono, bubbleSpawner, bubbleCluster, inputManager, clapModel));

        }

        // チュートリアルを開始する
        public void StartTutorial(Action callback)
        {
            _onComplete = callback;

            _isInTutorial = true;
            _currentTutorialIndex = 0;
            // チュートリアル内容がなければ
            if (tutorialContexts.Count == 0)
            {
                FinishTutorial();
                return;
            }
            // 最初のスタート
            tutorialContexts[_currentTutorialIndex].OnStart();
        }

        // チュートリアルを一つ進める
        void ProcessTutorial()
        {
            if (_currentTutorialIndex < 0) return;

            // TutorialContextのOnCompleteを呼び出す
            if (_currentTutorialIndex < tutorialContexts.Count)
            {
                tutorialContexts[_currentTutorialIndex].OnComplete();
            }
            // 次のOnStartを呼び出す
            _currentTutorialIndex++;
            if (_currentTutorialIndex < tutorialContexts.Count)
            {
                tutorialContexts[_currentTutorialIndex].OnStart();
            }
            else
            {
                FinishTutorial();
            }
        }

        void FinishTutorial()
        {
            _isInTutorial = false;
            _onComplete?.Invoke();
        }


        void ITickable.Tick()
        {
            if (!_isInTutorial) return;

            // 現在のチュートリアル更新
            tutorialContexts[_currentTutorialIndex].Update();

            // 終了したら
            if (tutorialContexts[_currentTutorialIndex].IsFinish)
            {
                ProcessTutorial();
            }
        }
    }
}
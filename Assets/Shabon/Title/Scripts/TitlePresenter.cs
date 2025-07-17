#nullable enable

using UnityEngine;
using Shabon.Utility;
using VContainer;
using VContainer.Unity;
using Shabon.Input;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shabon.Title
{
    /// <summary>
    /// タイトルシーンのViewとModelを橋渡しするクラス
    /// </summary>
    public class TitlePresenter : IInitializable, IDisposable
    {
        // skipコマンド用
        private enum Command
        {
            Clap,
            Breath
        }

        private List<IDisposable> _disposables = new();

        // ブレスした時間
        private float _breathContinuousTime = 0f;
        private List<Command> _skipInputCommand = new List<Command>();
        private readonly List<Command> _skipCorrectCommand = new List<Command>() { Command.Clap, Command.Breath, Command.Clap}; //skipのコマンド
        private bool _isStartBreath = false;

        [Inject]
        public TitlePresenter(
            TitleViewMono titleViewMono,
            IInputManager inputManager,
            TitleModel titleModel,
            SelectLanguageViewMono selectLanguageViewMono)
        {
            // Model -> View
            _disposables.Add(Observable.EveryValueChanged(titleModel, t => t.CurrentState)
                .Subscribe(state =>
                {
                    if (state == TitleState.Language)
                    {
                        selectLanguageViewMono.Open();
                    }
                    else if (state == TitleState.Prologue)
                    {
                        selectLanguageViewMono.Close();
                        titleViewMono.StartPrologue();
                    }
                })
            );
            _disposables.Add(Observable.EveryValueChanged(titleModel, t => t.CurrentLanguage)
                .Subscribe(language =>
                {
                    if (titleModel.CurrentState == TitleState.Language)
                    {
                        selectLanguageViewMono.SetLanguage(language);
                    }
                })
            );

            // View -> Model

            // プロローグが終われば、シーン遷移
            titleViewMono.ProloguePlayableDirector.stopped += director =>
                {
                    SceneTransition.Transition(SceneName.GameScene);
                    // asyncSceneLoaderMono.LoadGameScene();
                };


            // 入力取得
            _disposables.Add(Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    bool isClap = inputManager.GetClap();
                    float horizontal = inputManager.GetHorizontalDirection();
                    float breath = inputManager.GetBreath();

                    switch (titleModel.CurrentState)
                    {
                        case TitleState.Start:
                            // Clapされればゲーム開始
                            if (isClap) titleModel.StartGame();
                            
                            break;

                        case TitleState.Language:
                            if (horizontal >= 0.2)
                            {
                                titleModel.SetLanguage(Language.English);
                            }
                            else if (horizontal <= -0.2)
                            {
                                titleModel.SetLanguage(Language.Japanese);
                            }
                            // 息をした時間を加算
                            if (breath > 0)
                            {
                                _breathContinuousTime += Time.deltaTime;
                            }
                            else
                            {
                                _breathContinuousTime = 0;
                            }
                            selectLanguageViewMono.SetBreath(_breathContinuousTime);
                            // 1秒連続で吹いたら
                            if (_breathContinuousTime > 1)
                            {
                                titleModel.DecideLanguage();
                            }
                            break;

                        case TitleState.Prologue:
                            // breathの入力
                            if (!_isStartBreath && breath > 0)
                            {
                                _isStartBreath = true;
                            }
                            else if (_isStartBreath && breath == 0)
                            {
                                _skipInputCommand.Add(Command.Breath);
                                _isStartBreath = false;
                            }

                            // clapの入力
                            if (isClap) _skipInputCommand.Add(Command.Clap);

                            // パターンマッチング
                            if (_skipInputCommand.SequenceEqual(_skipCorrectCommand.Take(_skipInputCommand.Count)))
                            {
                                // 全て一致すればスキップ
                                if (_skipInputCommand.Count == _skipCorrectCommand.Count)
                                    titleViewMono.ProloguePlayableDirector.Stop();
                            }
                            else
                            {
                                _skipInputCommand.Clear();
                            }
                            
                            break;

                    }
                })
            );
        }

        // このクラスを生成するためのエントリーポイント
        void IInitializable.Initialize()
        {

        }

        void IDisposable.Dispose()
        {
            foreach (var _disposable in _disposables)
            {
                _disposable.Dispose();
            }
        }
    }
}
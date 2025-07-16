#nullable enable

using UnityEngine;
using Shabon.Utility;
using VContainer;
using VContainer.Unity;
using Shabon.Input;
using R3;
using System;
using System.Collections.Generic;

namespace Shabon.Title
{
    /// <summary>
    /// タイトルシーンのViewとModelを橋渡しするクラス
    /// </summary>
    public class TitlePresenter : IInitializable, IDisposable
    {
        private List<IDisposable> _disposables = new();

        // ブレスした時間
        private float _breathContinuousTime = 0f;
        [Inject]
        public TitlePresenter(TitleViewMono titleViewMono,
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
            titleViewMono.ProloguePlayableDirector.stopped +=
                director => SceneTransition.Transition(SceneName.GameScene);


            // 入力取得
            _disposables.Add(Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    bool isClap = inputManager.GetClap();
                    float horizontal = inputManager.GetHorizontalDirection();
                    float breath = inputManager.GetBreath();
                    // Start
                    if (titleModel.CurrentState == TitleState.Start)
                    {
                        // Clapされればゲーム開始
                        if (isClap)
                        {
                            titleModel.StartGame();
                        }
                    }
                    // 言語選択
                    // 英語と日本語を切り替え
                    else if (titleModel.CurrentState == TitleState.Language)
                    {
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
                        // 1秒連続で吹いたら
                        if (_breathContinuousTime > 1)
                        {
                            titleModel.DecideLanguage();
                        }

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
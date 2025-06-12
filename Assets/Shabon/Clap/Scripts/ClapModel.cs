using System;
using UnityEngine;
using Shabon.Bubble;
using VContainer;
using R3;

namespace Shabon.Clap
{
    public class ClapModel
    {
        private readonly IBubbleHandler _bubbleHandler;
        private bool _canClap = true; // Clap可能かどうかを管理するフラグ
        private const float ClapCoolTime = 5f; // クールダウン時間
        private readonly CompositeDisposable _disposable = new();

        [Inject]
        public ClapModel(IBubbleHandler bubbleHandler)
        {
            _bubbleHandler = bubbleHandler;
        }

        public void ApplyClap(float strength)
        {
            if (_canClap)
            {
                _bubbleHandler.ApplyClap(strength);
                _canClap = false; // Clapを使用不可に設定

                // Observable.Timerを使用してクールタイムを管理
                Observable.Timer(TimeSpan.FromSeconds(ClapCoolTime))
                    .Subscribe(_ => ResetClap())
                    .AddTo(_disposable);
            }

        }

        private void ResetClap()
        {
            _canClap = true;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
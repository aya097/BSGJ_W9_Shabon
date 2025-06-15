using System;
using UnityEngine;
using Shabon.Bubble;
using VContainer;
using R3;

namespace Shabon.Clap
{
    public class ClapModel
    {
        public float CoolTime => ClapCoolTime;
        public float CurrentTime => _currentTime;
        private readonly IBubbleHandler _bubbleHandler;
        private bool _canClap = true; // Clap可能かどうかを管理するフラグ
        private const float ClapCoolTime = 5f; // クールダウン時間
        private float _currentTime = 0f;
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
                _currentTime = 0f;

                // Observable.Timerを使用してクールタイムを管理

                Observable.Timer(TimeSpan.FromSeconds(ClapCoolTime))
                    .Subscribe(_ => ResetClap())
                    .AddTo(_disposable);
                Observable.EveryUpdate()
                    .TakeUntil(_ => _currentTime >= 5f)
                    .Subscribe(_ => _currentTime += Time.deltaTime)
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
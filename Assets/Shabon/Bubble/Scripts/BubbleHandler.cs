using UnityEngine;
using System;
using Shabon.Bubble;
using Shabon.Breath;
using System.Collections.Generic;
using VContainer;
using System.Linq;
using System.Threading.Tasks; // 非同期処理用

namespace Shabon.Bubble
{
    public class BubbleHandler : IBubbleHandler
    {
        private readonly BreathGetterViewMono _breathGetter;

        [Inject]
        public BubbleHandler(BreathGetterViewMono breathGetterViewMono)
        {
            _breathGetter = breathGetterViewMono;
        }

        public void ApplyBreath(Vector3 direction, Vector3 position, float strength)
        {
            if (strength == 0) return;
            foreach (var bubbleMono in _breathGetter.GetBubbleMonos())
            {
                bubbleMono.InvokeOnBreath(new OnBreathArg(strength, direction, position));
            }
        }

        public async void ApplyClap(Vector3 position, float strength)
        {
            // x座標が -0.2 から 0.2 の範囲にいるバブルを取得
            //手前にあるBubbleから倒す
            var bubblesInRange = _breathGetter.GetBubbleMonos()
                .Where(b => b.Transform.position.x >= -0.2 && b.Transform.position.x <= 0.2)
                .OrderBy(b => b.Transform.position.z)
                .ToList();

            foreach (var bubble in bubblesInRange)
            {
                // バブルを消滅させる
                bubble.InvokeOnDead();

                // 次のバブルを消すまで少し待機
                await Task.Delay(200); // 200ms 待機（必要に応じて調整）
            }
        }
    }
}
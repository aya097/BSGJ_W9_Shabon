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

        // Clapの範囲
        public float ClapRangeMinX { get; set; } = -2.0f;
        public float ClapRangeMaxX { get; set; } = 2.0f;

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

        public void ApplyClap(Vector3 position, float strength)
        {
            // Clap範囲内のバブルを取得し、z軸でソート
            var bubblesInRange = _breathGetter.GetBubbleMonos()
                .Where(b => b.Transform.position.x >= ClapRangeMinX && b.Transform.position.x <= ClapRangeMaxX)
                .OrderBy(b => b.Transform.position.z);

            // Clap処理を実行
            foreach (var bubble in bubblesInRange)
            {
                bubble.InvokeOnClap(new OnClapArg(strength));
            }
        }
    }
}
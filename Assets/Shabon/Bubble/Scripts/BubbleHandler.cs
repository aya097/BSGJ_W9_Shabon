using UnityEngine;
using System;
using Shabon.Bubble;
using Shabon.Breath;
using System.Collections.Generic;
using VContainer;
using System.Linq;
using System.Threading.Tasks;
using Shabon.Clap; // 非同期処理用

namespace Shabon.Bubble
{
    public class BubbleHandler : IBubbleHandler
    {
        private readonly BreathGetterViewMono _breathGetter;
        private readonly ClapGetterViewMono _clapGetter;


        [Inject]
        public BubbleHandler(BreathGetterViewMono breathGetterViewMono,
            ClapGetterViewMono clapGetterViewMono)
        {
            _breathGetter = breathGetterViewMono;
            _clapGetter = clapGetterViewMono;
        }

        public void ApplyBreath(Vector3 direction, Vector3 position, float strength)
        {
            if (strength == 0) return;
            foreach (var bubbleMono in _breathGetter.GetBubbleMonos())
            {
                bubbleMono.InvokeOnBreath(new OnBreathArg(strength, direction, position));
            }
        }

        public void ApplyClap(float strength)
        {
            if (strength == 0) return;
            var bubbleMono = _clapGetter.GetBubbleMonos().OrderBy(b => b.Transform.position.z).FirstOrDefault();    // 一番近いバブル
            if (bubbleMono != null)
            {
                bubbleMono.InvokeOnClap(new OnClapArg(strength));
            }
        }
    }
}
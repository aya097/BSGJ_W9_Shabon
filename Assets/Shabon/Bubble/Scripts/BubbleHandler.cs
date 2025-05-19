using UnityEngine;
using System;
using Shabon.Bubble;
using Shabon.Breath;
using System.Collections.Generic;
using VContainer;

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

        public void ApplyClap(Vector3 position, float strength)
        {

        }

    }
}
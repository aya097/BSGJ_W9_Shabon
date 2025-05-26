using System.Collections.Generic;
using UnityEngine;
using Shabon.Bubble;

namespace Shabon.Clap
{
    public class ClapModel
    {
        private readonly IBubbleHandler _bubbleHandler;
        private readonly ClapGetterViewMono _clapGetter;

        public ClapModel(IBubbleHandler bubbleHandler, ClapGetterViewMono clapGetter)
        {
            _bubbleHandler = bubbleHandler;
            _clapGetter = clapGetter;
        }

        public void ExecuteClap(float strength)
        {
            IEnumerable<IBubbleMono> bubbles = _clapGetter.GetBubbleMonos();


            _bubbleHandler.ApplyClap(bubbles, strength);
        }
    }
}
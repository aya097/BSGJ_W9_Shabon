using System.Collections.Generic;
using UnityEngine;
using Shabon.Bubble;
using VContainer;

namespace Shabon.Clap
{
    public class ClapModel
    {
        private readonly IBubbleHandler _bubbleHandler;
        [Inject]
        public ClapModel(IBubbleHandler bubbleHandler)
        {
            _bubbleHandler = bubbleHandler;
        }

        public void ApplyClap(float strength)
        {
            _bubbleHandler.ApplyClap(strength);
        }
    }
}
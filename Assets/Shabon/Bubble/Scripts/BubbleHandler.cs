using UnityEngine;
using System;
using Shabon.Bubble;
using Shabon.Breath;
using System.Collections.Generic;
using VContainer;
using System.Linq;
using System.Threading.Tasks;
using Shabon.Clap; // 非同期処理用
using Shabon.Score;
using UnityEditor.Localization.Plugins.XLIFF.V20;

namespace Shabon.Bubble
{
    public class BubbleHandler : IBubbleHandler
    {
        private readonly BreathGetterViewMono _breathGetter;
        private readonly BubbleCluster _bubbleCluster;
        private readonly IScoreValue _scoreValue;


        [Inject]
        public BubbleHandler(BreathGetterViewMono breathGetterViewMono,
            BubbleCluster bubbleCluster,
            IScoreValue scoreValue)
        {
            _breathGetter = breathGetterViewMono;
            _bubbleCluster = bubbleCluster;
            _scoreValue = scoreValue;
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

            // 範囲内のすべてのバブルを取得
            var bubblesInRange = _bubbleCluster.Bubbles.Where(b => b.IsClapable);

            // 倒したバブルの数をカウント
            int defeatedCount = 0;

            foreach (var bubbleMono in bubblesInRange)
            {
                bubbleMono.InvokeOnClap(new OnClapArg(strength));
                defeatedCount++;
            }

            // 同時に倒した数に応じてスコアを増加
            if (defeatedCount > 0)
            {
                _scoreValue.Increase(defeatedCount * 10);
            }
        }
    }
}
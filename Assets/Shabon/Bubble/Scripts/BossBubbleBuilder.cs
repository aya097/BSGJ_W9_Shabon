#nullable enable

using System;
using System.Collections.Generic;
using R3;
using Shabon.Param;
using Shabon.Score;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

namespace Shabon.Bubble
{
    /// <summary>
    /// BossBubbleの個性を付与するクラス
    /// </summary>
    public class BossBubbleBuilder : NormalBubbleBuilder, IBubbleBuilder
    {
        private readonly IPlayerTransform _playerTransform;
        private readonly BubbleCluster _bubbleCluster;
        private readonly IDirtValue _dirtValue;
        private readonly IAreaChecker _waitAreaChecker;
        private readonly IBubbleCombo _bubbleCombo;
        private readonly IScoreValue _scoreValue;
        private readonly List<IDisposable> _presenterObservable = new();

        [Inject]
        public BossBubbleBuilder(
            IPlayerTransform playerTransform,
            BubbleCluster bubbleCluster,
            IDirtValue dirtValue,
            IAreaChecker waitAreaChecker,
            IBubbleCombo bubbleCombo,
            IScoreValue scoreValue)
            : base(playerTransform, bubbleCluster, dirtValue, waitAreaChecker, bubbleCombo, scoreValue)
        {
            _playerTransform = playerTransform;
            _bubbleCluster = bubbleCluster;
            _dirtValue = dirtValue;
            _waitAreaChecker = waitAreaChecker;
            _bubbleCombo = bubbleCombo;
            _scoreValue = scoreValue;
        }

        /// <summary>
        /// 個性を付与するメソッド
        /// </summary>
        public override void Build(IBubbleBuildSetter bubbleSetter, IBubbleMono bubbleMono, IBubbleData bubbleData, BubbleViewMono bubbleViewMono)
        {
            base.Build(bubbleSetter, bubbleMono, bubbleData, bubbleViewMono);

            // Deadの処理
            DeathParams deathParams = new DeathParams(_scoreValue, _dirtValue, _bubbleCombo);
            BubbleDeath bubbleDeath = new BubbleDeath(
                BubbleType.Boss,
                deathParams,
                () => { DestroyBubble(bubbleMono); });

        }

    }
}

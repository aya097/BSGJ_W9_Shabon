#nullable enable

using Shabon.Param;
using Shabon.Score;
using UnityEngine;
using VContainer;

namespace Shabon.Bubble
{
    /// <summary>
    /// ArmorBubbleの個性を付与するクラス
    /// </summary>
    public class ArmorBubbleBuilder : NormalBubbleBuilder
    {
        private readonly IPlayerTransform _playerTransform;
        private readonly BubbleCluster _bubbleCluster;
        private readonly IDirtValue _dirtValue;
        private readonly IAreaChecker _waitAreaChecker;
        private readonly IBubbleCombo _bubbleCombo;
        private readonly IScoreValue _scoreValue;

        [Inject]
        public ArmorBubbleBuilder(
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

        public override void Build(
            IBubbleBuildSetter bubbleSetter,
            IBubbleMono bubbleMono,
            IBubbleData bubbleData,
            BubbleViewMono bubbleViewMono)
        {
            base.Build(bubbleSetter, bubbleMono, bubbleData, bubbleViewMono);

            DeathParams deathParams = new DeathParams(_scoreValue, _dirtValue, _bubbleCombo);
            BubbleDeath bubbleDeath = new BubbleDeath(
                BubbleType.Armor,
                deathParams,
                () => { DestroyBubble(bubbleMono); });

            SetOnDead(bubbleMono, bubbleDeath, bubbleViewMono);
        }


        protected override void SetOnBreath(IBubbleMono bubbleMono, IBubbleBuildSetter bubbleSetter, IBubbleMover bubbleMover, Transform bubbleTransform, BubbleViewMono bubbleView)
        {
            bubbleSetter.OnBreath += (arg) =>
                {
                    // 到達してないときかつ動かないとき
                    if (!(bubbleMono.IsReached || bubbleMono.IsStop))
                    {
                        bubbleMono.IsBreathing = true;

                        // 息が吹かれた時のアニメーションを再生
                        bubbleView.PlayBreath(bubbleMono);
                        bubbleView.SetHighlight(HighLightType.Breathed);
                    }
                };
        }


        /// <summary>
        /// Clapされた時の処理
        /// </summary>
        protected override void SetOnClap(IBubbleMono bubbleMono, IBubbleBuildSetter bubbleSetter, BubbleDeath bubbleDeath, BubbleViewMono bubbleView)
        {
            bubbleSetter.OnClap += _ =>
            {
                bubbleView.SetHighlight(HighLightType.Claped); 
                bubbleView.PlayClap(() =>
                {
                    bubbleView.SetHighlight(HighLightType.Guard);
                 }, false);
            };
        }

        /// <summary>
        /// 死んだときの処理
        /// <summary>
        private void SetOnDead(IBubbleMono bubbleMono, BubbleDeath bubbleDeath, BubbleViewMono bubbleView)
        {
            if (bubbleMono is ArmorBubbleMono armorBubbleMono)
            {
                armorBubbleMono.OnDead += () =>
                {
                    bubbleMono.Stop();
                    bubbleView.SetHighlight(HighLightType.Attack);
                    bubbleView.PlayClap(() =>
                    {
                        bubbleDeath.InvokeDeath(BubbleDeathType.Clap);
                    });
                };
            }

        }

    }
}

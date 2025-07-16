#nullable enable

using System;
using System.Collections.Generic;
using R3;
using Shabon.Game;
using Shabon.Param;
using Shabon.Score;
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
        private readonly IBubbleSpawner _bubbleSpawner;

        private float _bossBattleStartTime = 0f;
        protected override bool EnableSpreading => false;

        [Inject]
        public BossBubbleBuilder(
            IPlayerTransform playerTransform,
            BubbleCluster bubbleCluster,
            IDirtValue dirtValue,
            IAreaChecker waitAreaChecker,
            IBubbleCombo bubbleCombo,
            IScoreValue scoreValue,
            IBubbleSpawner bubbleSpawner)
            : base(playerTransform, bubbleCluster, dirtValue, waitAreaChecker, bubbleCombo, scoreValue)
        {
            _playerTransform = playerTransform;
            _bubbleCluster = bubbleCluster;
            _dirtValue = dirtValue;
            _waitAreaChecker = waitAreaChecker;
            _bubbleCombo = bubbleCombo;
            _scoreValue = scoreValue;
            _bubbleSpawner = bubbleSpawner;
        }

        public void SetBossBattleStartTime(float time)
        {
            _bossBattleStartTime = time;
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

            if (bubbleViewMono is BossBubbleViewMono bossBubbleView)
                SetOnSpawn(bubbleSetter, bubbleMono, bossBubbleView);

        }

        protected override void SetOnBreath(IBubbleMono bubbleMono, IBubbleBuildSetter bubbleSetter, IBubbleMover bubbleMover, Transform bubbleTransform, BubbleViewMono bubbleView)
        {
            bubbleSetter.OnBreath += (arg) =>
            {

            };
        }

        protected override void SetOnClap(IBubbleMono bubbleMono, IBubbleBuildSetter bubbleSetter, BubbleDeath bubbleDeath, BubbleViewMono bubbleView)
        {
            bubbleSetter.OnClap += _ =>
            {
                // 攻撃中は倒せない
                if (bubbleMono.IsAttacking) return;

                bubbleMono.Stop();
                bubbleView.SetHighlight(HighLightType.Claped);
                bubbleMono.DecreaseHp(1);
                // animatorに反映させる
                if (bubbleView is BossBubbleViewMono bossBubbleView)
                    bossBubbleView.SetBossHp(bubbleMono.BossHitPoint);

                bubbleView.PlayClap(() =>
                {
                    
                    if (bubbleMono.BossHitPoint == 0)
                    {
                        // ボス撃破時
                        float bossBattleTime = Time.time - PhaseExecutor.BossBattleStartTime;
                        ResultData.BossBattleTime = bossBattleTime;
                        bubbleDeath.InvokeDeath(BubbleDeathType.Clap);
                        return;
                    }

                    bubbleMono.Resume();
                });

            };
        }

        protected override void SetOnReach(IBubbleBuildSetter bubbleSetter, IBubbleMono bubbleMono, IBubbleData bubbleData, BubbleDeath bubbleDeath, BubbleViewMono bubbleView)
        {
            bubbleSetter.OnReach += () =>
            {
                Observable.Timer(TimeSpan.FromSeconds(bubbleData.ZoneWaitingTime))
                    .Subscribe(_ =>
                    {
                        if ((bubbleMono as MonoBehaviour) != null)
                        {
                            bubbleMono.IsAttacking = true;
                            bubbleView.SetHighlight(HighLightType.Attack);
                            bubbleView.PlayAttack(() =>
                            {
                                _dirtValue.Increase(bubbleData.IncreasingDirtValue);
                                bubbleMono.IsAttacking = false;
                                bubbleMono.IsReached = false;
                                bubbleMono.Back();
                            });
                        }
                    });
            };
        }

        // バブルを呼び出す処理
        private void SetOnSpawn(IBubbleBuildSetter bubbleSetter, IBubbleMono bubbleMono, BossBubbleViewMono bossBubbleView)
        {
            if (bubbleSetter is BossBubbleMono bossBubbleSetter)
            {
                bossBubbleSetter.OnSpawn += (bubbleTypeList) =>
                {
                    bubbleMono.Stop();
                    bossBubbleView.SetHighlight(HighLightType.None); // ここ仮
                    bossBubbleView.PlaySpawn(() =>
                        {
                            foreach (BubbleType bubbleType in bubbleTypeList)
                            {
                                _bubbleSpawner.Spawn(bubbleType);
                            }
                            bubbleMono.Resume();
                        }
                    );
                };
            }

        }
    }
}

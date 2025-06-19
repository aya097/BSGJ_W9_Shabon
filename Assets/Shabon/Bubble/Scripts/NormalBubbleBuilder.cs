#nullable enable

using System;
using System.Collections.Generic;
using R3;
using Shabon.Param;
using Shabon.Score;
using UnityEngine;
using VContainer;

namespace Shabon.Bubble
{
    /// <summary>
    /// NormalBubbleの個性を付与するクラス
    /// </summary>
    public class NormalBubbleBuilder : IBubbleBuilder
    {
        private readonly IPlayerTransform _playerTransform;
        private readonly BubbleCluster _bubbleCluster;
        private readonly IDirtValue _dirtValue;
        private readonly IAreaChecker _waitAreaChecker;
        private readonly IBubbleCombo _bubbleCombo;
        private readonly IScoreValue _scoreValue;
        private readonly List<IDisposable> _presenterObservable = new();

        [Inject]
        public NormalBubbleBuilder(
            IPlayerTransform playerTransform,
            BubbleCluster bubbleCluster,
            IDirtValue dirtValue,
            IAreaChecker waitAreaChecker,
            IBubbleCombo bubbleCombo,
            IScoreValue scoreValue)
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
        public void Build(
            IBubbleBuildSetter bubbleSetter,
            IBubbleMono bubbleMono,
            IBubbleData bubbleData,
            BubbleViewMono bubbleViewMono)
        {
            // BubbleMoverの生成
            IBubbleMover bubbleMover = GetBubbleMover(bubbleMono.Transform, bubbleData);

            // Deadの処理
            DeathParams deathParams = new DeathParams(_scoreValue, _dirtValue, _bubbleCombo);
            BubbleDeath bubbleDeath = new BubbleDeath(
                BubbleType.Normal,
                deathParams,
                () => { DestroyBubble(bubbleMono); });

            // Breathの処理
            SetOnBreath(bubbleMono, bubbleSetter, bubbleMover, bubbleMono.Transform, bubbleViewMono);

            // Clapの処理
            SetOnClap(bubbleMono, bubbleSetter, bubbleDeath, bubbleViewMono);

            // Reachの処理
            SetOnReach(bubbleSetter, bubbleMono, bubbleData, bubbleDeath, bubbleViewMono);

            bubbleSetter.SetBuildParam(bubbleMover, bubbleDeath, _waitAreaChecker, bubbleData);

            // プレゼンター処理？
            Observable.EveryValueChanged(bubbleMono, b => b.IsClapable)
                .Subscribe(clapable =>
                {
                    if (clapable)
                    {
                        bubbleViewMono.TurnOnHighlight();
                    }
                    else
                    {
                        bubbleViewMono.TurnOffHighlight();
                    }
                }).AddTo(bubbleViewMono);
        }

        /// <summary>
        /// 適切なBubbleMoverを返す
        /// </summary>
        private IBubbleMover GetBubbleMover(Transform transform, IBubbleData bubbleData)
        {
            float forwardVelocity = bubbleData.ForwardVelocity;
            return bubbleData.BubbleType switch
            {
                BubbleType.Normal => new NormalBubbleMover(transform, forwardVelocity, _playerTransform.PlayerTransform),
                _ => new NormalBubbleMover(transform, forwardVelocity, _playerTransform.PlayerTransform) // もし該当がなければNormalを返しておく
            };
        }
        /// <summary>
        /// 息を吹かれたときの処理を作成
        /// </summary>
        private void SetOnBreath(IBubbleMono bubbleMono, IBubbleBuildSetter bubbleSetter, IBubbleMover bubbleMover, Transform bubbleTransform, BubbleViewMono bubbleViewMono)
        {
            bubbleSetter.OnBreath += (arg) =>
            {
                // 到達してないときかつ動かないとき
                if (!(bubbleMono.IsReached || bubbleMono.IsStop))
                {
                    // 息が吹かれた時のアニメーションを再生
                    bubbleViewMono.PlayBreath();

                    // Playerと逆の方向
                    Vector3 moveDirection = bubbleTransform.position - _playerTransform.PlayerTransform.position;
                    moveDirection.y = 0;
                    bubbleMover.MoveByBreath(moveDirection.normalized * arg.Strength);
                }
            };
        }

        /// <summary>
        /// Clapされた時の処理
        /// </summary>
        private void SetOnClap(IBubbleMono bubbleMono, IBubbleBuildSetter bubbleSetter, BubbleDeath bubbleDeath, BubbleViewMono bubbleView)
        {
            bubbleSetter.OnClap += _ =>
            {
                // 攻撃中は倒せない
                if (!bubbleMono.IsAttacking)
                {
                    bubbleMono.Stop();
                    bubbleView.PlayClap(() =>
                    {
                        bubbleDeath.InvokeDeath(BubbleDeathType.Clap);
                    });
                }
            };
        }

        /// <summary>
        /// エリアに到達したときの処理
        /// </summary>
        private void SetOnReach(IBubbleBuildSetter bubbleSetter, IBubbleMono bubbleMono, IBubbleData bubbleData, BubbleDeath bubbleDeath, BubbleViewMono bubbleView)
        {
            bubbleSetter.OnReach += () =>
            {
                // 待機時間後にdestroy
                Observable.Timer(TimeSpan.FromSeconds(bubbleData.ZoneWaitingTime))
                    .Subscribe(_ =>
                    {
                        if ((bubbleMono as MonoBehaviour) != null)
                        {
                            bubbleMono.IsAttacking = true;
                            bubbleView.PlayAttack(() => bubbleDeath.InvokeDeath(BubbleDeathType.Attack));
                        }
                    });
            };
        }

        /// <summary>
        /// BubbleをDestroyする用の関数、これ以外ではDestroyしてはいけない
        /// </summary>
        /// <param name="bubbleMono"></param>
        private void DestroyBubble(IBubbleMono bubbleMono)
        {
            // Clusterから削除
            _bubbleCluster.Remove(bubbleMono);

            // Destroy
            GameObject.Destroy(bubbleMono.Transform?.gameObject);
        }
    }
}

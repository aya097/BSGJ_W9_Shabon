#nullable enable

using System;
using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using R3;
using Shabon.Param;
using Shabon.Score;
using Unity.Mathematics;
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
        public virtual void Build(
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

            // 広がる処理
            // 最初は背景より後ろに描画
            bubbleViewMono.SetSortingLayer("Back");
            bubbleMono.IsSeparatable = false;  // 広がるまで停止
            SpreadBubble(bubbleMono, bubbleViewMono);


            bubbleSetter.SetBuildParam(bubbleMover, _waitAreaChecker, bubbleData, _bubbleCluster);

            // プレゼンター処理？
            Observable.EveryValueChanged(bubbleMono, b => b.IsClapable)
                .Subscribe(clapable =>
                {
                    if (bubbleData.BubbleType == BubbleType.Armor) return;
                    if (clapable)
                    {
                        bubbleViewMono.SetHighlight(HighLightType.Clapable);
                    }
                    else
                    {
                        bubbleViewMono.SetHighlight(HighLightType.None);
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
                BubbleType.Boss => new BossBubbleMover(transform, forwardVelocity, _playerTransform.PlayerTransform),
                _ => new NormalBubbleMover(transform, forwardVelocity, _playerTransform.PlayerTransform) // もし該当がなければNormalを返しておく
            };
        }

        /// <summary>
        /// バブルが最初に広がる処理
        /// </summary>
        private void SpreadBubble(IBubbleMono bubbleMono, BubbleViewMono viewMono)
        {
            // 移動する目的地(x)

            float targetPosition = UnityEngine.Random.Range(0, 1.5f) * Mathf.Sign(bubbleMono.Transform.position.x);

            // n秒後に実行（ポータルから出たときに動きたい）
            Observable.EveryUpdate()
                .Where(_ => bubbleMono.Transform.position.z < 0.95f)
                .Take(1)
                .Subscribe(_ =>
                {
                    // 描画を優先する
                    viewMono.SetSortingLayer("Bubble");
                    // 0.5秒で移動する
                    LMotion.Create(bubbleMono.Transform.position.x, targetPosition, 0.5f)
                        .WithOnComplete(() => bubbleMono.IsSeparatable = true)
                        .WithEase(Ease.OutSine)
                        .BindToPositionX(bubbleMono.Transform)
                        .AddTo(viewMono);
                })
                .AddTo(viewMono);   // 寿命管理のため
        }

        /// <summary>
        /// 息を吹かれたときの処理を作成
        /// </summary>
        protected virtual void SetOnBreath(IBubbleMono bubbleMono, IBubbleBuildSetter bubbleSetter, IBubbleMover bubbleMover, Transform bubbleTransform, BubbleViewMono bubbleView)
        {


            // // 4秒かけて-1.3f〜1.3fの間でランダムな位置に横移動しつつ、前進も行う
            // float randomX = UnityEngine.Random.Range(-1.3f, 1.3f);
            // var moveTime = 4f;
            // var elapsed = 0f;

            // bubbleMono.Transform.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(MoveSideAndForwardCoroutine());

            // System.Collections.IEnumerator MoveSideAndForwardCoroutine()
            // {
            //     Vector3 startPos = bubbleMono.Transform.position;
            //     Vector3 targetPos = startPos + bubbleMono.Transform.right * randomX;

            //     while (elapsed < moveTime)
            //     {
            //         elapsed += Time.deltaTime;
            //         float t = Mathf.Clamp01(elapsed / moveTime);

            //         // 横移動
            //         Vector3 sidePos = Vector3.Lerp(startPos, targetPos, t);

            //         // デフォルトの速度で進む
            //         Vector3 forwardDir = (_playerTransform.PlayerTransform.position - bubbleMono.Transform.position).normalized;
            //         forwardDir.y = 0;
            //         float forwardSpeed = (bubbleMover is NormalBubbleMover normalMover)
            //             ? typeof(NormalBubbleMover).GetField("_forwardVelocity", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(normalMover) as float? ?? 0f
            //             : 0f;

            //         sidePos += forwardDir * forwardSpeed * elapsed;

            //         bubbleMono.Transform.position = sidePos;
            //         yield return null;
            //     }
            // }

            // 通常の息処理
            bubbleSetter.OnBreath += (arg) =>
            {
                // 到達してないときかつ動かないとき
                if (!(bubbleMono.IsReached || bubbleMono.IsStop))
                {
                    bubbleMono.IsBreathing = true;

                    // 息が吹かれた時のアニメーションを再生
                    bubbleView.PlayBreath(bubbleMono);
                    bubbleView.SetHighlight(HighLightType.Breathed);

                    // Playerと逆の方向
                    Vector3 moveDirection = bubbleTransform.position - _playerTransform.PlayerTransform.position;
                    moveDirection.y = 0;
                    moveDirection = moveDirection.normalized * arg.Strength / (bubbleTransform.position - arg.Position).magnitude;  // Breathの強さに比例、距離に反比例
                    bubbleMover.MoveByBreath(moveDirection);

                }
            };
        }

        /// <summary>
        /// Clapされた時の処理
        /// </summary>
        protected virtual void SetOnClap(IBubbleMono bubbleMono, IBubbleBuildSetter bubbleSetter, BubbleDeath bubbleDeath, BubbleViewMono bubbleView)
        {
            bubbleSetter.OnClap += _ =>
            {
                // 攻撃中は倒せない
                if (!bubbleMono.IsAttacking)
                {
                    bubbleMono.Stop();
                    bubbleView.SetHighlight(HighLightType.Claped);
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
        protected virtual void SetOnReach(IBubbleBuildSetter bubbleSetter, IBubbleMono bubbleMono, IBubbleData bubbleData, BubbleDeath bubbleDeath, BubbleViewMono bubbleView)
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
                            bubbleView.SetHighlight(HighLightType.Attack);
                            bubbleView.PlayAttack(() => bubbleDeath.InvokeDeath(BubbleDeathType.Attack));
                        }
                    });
            };
        }

        /// <summary>
        /// BubbleをDestroyする用の関数、これ以外ではDestroyしてはいけない
        /// </summary>
        /// <param name="bubbleMono"></param>
        protected virtual void DestroyBubble(IBubbleMono bubbleMono)
        {
            // Clusterから削除
            _bubbleCluster.Remove(bubbleMono);

            // Destroy
            GameObject.Destroy(bubbleMono.Transform?.gameObject);
        }
    }
}

using System;
using R3;
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
        private readonly BubbleCluster _bubbleCluster;
        private readonly IDirtValue _dirtValue;
        private readonly IAreaChecker _waitAreaChecker;
        private readonly IBubbleChain _bubbleChain;

        [Inject]
        public NormalBubbleBuilder(
            BubbleCluster bubbleCluster,
            IDirtValue dirtValue,
            IAreaChecker waitAreaChecker,
            IBubbleChain bubbleChain)
        {
            _bubbleCluster = bubbleCluster;
            _dirtValue = dirtValue;
            _waitAreaChecker = waitAreaChecker;
            _bubbleChain = bubbleChain;
        }
        /// <summary>
        /// 個性を付与するメソッド
        /// </summary>
        public void Build(
            IBubbleBuildSetter bubbleSetter,
            IBubbleMono bubbleMono,
            IBubbleData bubbleData)
        {
            // BubbleMoverの生成
            IBubbleMover bubbleMover = GetBubbleMover(bubbleMono.Transform, bubbleData);

            // Deadの処理
            SetOnDead(bubbleSetter, bubbleMono, bubbleData);

            // Breathの処理
            SetOnBreath(bubbleSetter, bubbleMover, bubbleMono.Transform);

            // Clapの処理
            SetOnClap(bubbleSetter, bubbleMono);

            // Reachの処理
            SetOnReach(bubbleSetter, bubbleMono, bubbleData);

            bubbleSetter.SetBuildParam(bubbleMover, _waitAreaChecker);
        }

        /// <summary>
        /// 適切なBubbleMoverを返す
        /// </summary>
        private IBubbleMover GetBubbleMover(Transform transform, IBubbleData bubbleData)
        {
            float forwardVelocity = bubbleData.ForwardVelocity;
            return bubbleData.BubbleType switch
            {
                BubbleType.Normal => new NormalBubbleMover(transform, forwardVelocity),
                _ => new NormalBubbleMover(transform, forwardVelocity)  // もし該当がなければNormalを返しておく
            };
        }
        /// <summary>
        /// 息を吹かれたときの処理を作成
        /// </summary>
        private void SetOnBreath(IBubbleBuildSetter bubbleSetter, IBubbleMover bubbleMover, Transform bubbleTransform)
        {
            bubbleSetter.OnBreath += (arg) =>
            {
                // y座標抜きの平面として扱って計算
                Vector2 bubblePosition = new Vector2(bubbleTransform.position.x, bubbleTransform.position.z); // Bubbleの座標
                Vector2 breathPosition = new Vector2(arg.Position.x, arg.Position.z);   // Breathの原点
                Vector2 breathDirection = new Vector2(arg.Direction.x, arg.Direction.z);   // Breathの向き 

                // y除算するため
                if (Mathf.Abs(breathDirection.y) < 0.01) breathDirection.y = 0.01f * Mathf.Sign(breathDirection.y);
                // x軸上でどれだけ離れているか
                float y = bubblePosition.y - breathPosition.y;
                float x = breathPosition.x + breathDirection.x / breathDirection.y * y;

                // Bubbleを移動
                Vector3 direction = new Vector3(bubblePosition.x - x, 0f, 0f);
                direction = direction.normalized * arg.Strength;
                bubbleMover.MoveByBreath(direction);
                Debug.Log(direction);
            };
        }

        /// <summary>
        /// 割られたときの処理
        /// </summary>
        private void SetOnDead(IBubbleBuildSetter bubbleSetter, IBubbleMono bubbleMono, IBubbleData bubbleData)
        {
            bubbleSetter.OnDead += () =>
            {
                DestroyBubble(bubbleMono);
                _bubbleChain.ExecuteBubbleChain(bubbleMono, bubbleData.ChainRadius);
            
            };
        }

        /// <summary>
        /// Clapされた時の処理
        /// </summary>
        private void SetOnClap(IBubbleBuildSetter bubbleSetter, IBubbleMono bubbleMono)
        {
            bubbleSetter.OnClap += _ =>
            {
                // Clapされたら割れる
                bubbleMono.InvokeOnDead();
            };
        }

        /// <summary>
        /// エリアに到達したときの処理
        /// </summary>
        private void SetOnReach(IBubbleBuildSetter bubbleSetter, IBubbleMono bubbleMono, IBubbleData bubbleData)
        {
            bubbleSetter.OnReach += () =>
            {
                // DirtValue増やす

                // 待機時間後にdestroy
                Observable.Timer(TimeSpan.FromSeconds(bubbleData.ZoneWaitingTime))
                    .Subscribe(_ =>
                    {
                        if (bubbleMono != null)
                        {
                            DestroyBubble(bubbleMono);
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
            GameObject.Destroy(bubbleMono.Transform.gameObject);
        }
    }
}

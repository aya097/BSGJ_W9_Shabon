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

        [Inject]
        public NormalBubbleBuilder(
            BubbleCluster bubbleCluster,
            IDirtValue dirtValue)
        {
            _bubbleCluster = bubbleCluster;
            _dirtValue = dirtValue;
        }
        /// <summary>
        /// 個性を付与するメソッド
        /// </summary>
        public void Build(
            IBubbleBuildSetter bubbleMono,
            IBubbleData bubbleData)
        {
            // BubbleMoverの生成
            IBubbleMover bubbleMover = GetBubbleMover(bubbleMono.Transform, bubbleData);

            // Breathの処理
            SetOnBreath(bubbleMono, bubbleMover);

            bubbleMono.SetBuildParam(bubbleMover);
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
        private void SetOnBreath(IBubbleBuildSetter bubbleMono, IBubbleMover bubbleMover)
        {
            bubbleMono.OnBreath += (arg) =>
            {
                // y座標抜きの平面として扱って計算
                Vector2 bubblePosition = new Vector2(bubbleMono.Transform.position.x, bubbleMono.Transform.position.z); // Bubbleの座標
                Vector2 breathPosition = new Vector2(arg.Position.x, arg.Position.z);   // Breathの原点
                Vector2 breathDirection = new Vector2(arg.Direction.x, arg.Direction.z);   // Breathの向き 

                // y除算するため
                if (Mathf.Abs(breathDirection.y) < 0.01) breathDirection.y = 0.01f * Mathf.Sign(breathDirection.y);
                // x軸上でどれだけ離れているか
                float y = bubblePosition.y - breathPosition.y;
                float x = breathPosition.x + breathDirection.x / breathDirection.y * y;

                // Bubbleを移動
                Vector3 direction = new Vector3(x, 0f, 0f);
                direction = direction.normalized * arg.Strength;
                bubbleMover.MoveByBreath(direction);
            };
        }

        /// <summary>
        /// 割られたときの処理
        /// </summary>
        private void SetOnDead(IBubbleBuildSetter bubbleMono)
        {
            bubbleMono.OnDead += () =>
            {
                // Clusterから削除
                _bubbleCluster.Remove(bubbleMono.Transform.GetComponent<BubbleMono>());

                // Destroy
                GameObject.Destroy(bubbleMono.Transform.gameObject);
            };
        }
    }
}

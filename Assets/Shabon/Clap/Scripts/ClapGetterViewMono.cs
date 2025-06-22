using UnityEngine;
#nullable enable
using System.Collections.Generic;
using System.Linq;
using Shabon.Bubble;
using VContainer;
using Unity.VisualScripting;

namespace Shabon.Clap
{
    public class ClapGetterViewMono : MonoBehaviour
    {
        [SerializeField] private float radius = 1.0f; // 円の半径を指定

        private BubbleCluster _bubbleCluster = null!;
        [Inject]
        void Initialize(BubbleCluster bubbleCluster)
        {
            _bubbleCluster = bubbleCluster;
        }

        void Update()
        {
            // ヒットしたバブル
            var hitBubbles = GetBubbleMonos();

            // ヒットしていないバブルを探す
            var notHitBubbles = _bubbleCluster.Bubbles.ToList();

            foreach (var bubble in hitBubbles)
            {
                notHitBubbles.Remove(bubble);
                // ヒットしたバブルはIsClapableをtrueにする
                bubble.IsClapable = true;
            }
            // ヒットしてないバブルのIsClapableをfalseにする
            foreach (var bubble in notHitBubbles)
            {
                bubble.IsClapable = false;
            }
        }

        // BubbleMonoを取得する
        private IEnumerable<IBubbleMono> GetBubbleMonos()
        {
            // 円形の範囲でColliderを取得
            Collider[] hits = Physics.OverlapSphere(transform.position, radius);

            // IBubbleMonoを取得し、nullでないものだけを集める
            return hits
                .Select(x =>
                {
                    var parent = x.transform.parent;
                    var bubbleMono = parent?.GetComponent<IBubbleMono>();
                    return bubbleMono;
                })
                .Where(x => x != null)
                .Select(x => x!);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f); // 半透明の赤色
            Gizmos.DrawSphere(transform.position, radius); // 範囲を描画
        }
    }
}

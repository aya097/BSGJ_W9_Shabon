#nullable enable
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
namespace Shabon.Bubble
{
    /// <summary>
    /// このクラスをアタッチしたオブジェクト内のBubbleMonoを取得する(Breath専用)
    /// </summary>
    public class BreathGetterViewMono : MonoBehaviour
    {
        // BubbleMonoを取得する
        public IEnumerable<IBubbleMono> GetBubbleMonos()
        {
            var boxCol = transform.GetComponent<BoxCollider>();
            Vector3 boxSize = new Vector3(transform.localScale.x * boxCol.size.x, transform.localScale.y * boxCol.size.y, transform.localScale.z * boxCol.size.z);  // Colliderのサイズ取得
            Collider[] hits = Physics.OverlapBox(transform.position, boxSize * 0.5f, transform.rotation);

            // IBubbleMonoを取得し、nullでないものだけを集めた。 x.transform.parentはBubbleの構造上無理矢理
            return hits.Select(x => x.transform.parent.GetComponent<IBubbleMono>()).Where(x => x != null).ToList();
        }
    }
}

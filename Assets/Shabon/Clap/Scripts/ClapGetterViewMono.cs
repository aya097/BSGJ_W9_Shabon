using UnityEngine;
#nullable enable
using System.Collections.Generic;
using System.Linq;
using Shabon.Bubble;

namespace Shabon.Clap
{
    public class ClapGetterViewMono : MonoBehaviour
    {
        // BubbleMonoを取得する
        public IEnumerable<IBubbleMono> GetBubbleMonos()
        {
            var boxCol = GetComponent<BoxCollider>();
            if (boxCol == null)
            {
                Debug.LogWarning($"BoxCollider が {gameObject.name} にアタッチされていません。");
                return Enumerable.Empty<IBubbleMono>();
            }

            Vector3 boxSize = new Vector3(
                transform.localScale.x * boxCol.size.x,
                transform.localScale.y * boxCol.size.y,
                transform.localScale.z * boxCol.size.z
            ); // Colliderのサイズ取得

            Collider[] hits = Physics.OverlapBox(transform.position, boxSize * 0.5f, transform.rotation);

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
    }
}

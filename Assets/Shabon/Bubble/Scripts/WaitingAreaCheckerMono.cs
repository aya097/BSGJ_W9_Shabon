#nullable enable 

using UnityEngine;

namespace Shabon.Bubble
{
    /// <summary>
    /// 範囲内にオブジェクトがいるか判定
    /// </summary>
    public class WaitingAreaCheckerMono : MonoBehaviour, IAreaChecker
    {
        [SerializeField] BoxCollider judgeArea = null!;

        public bool IsInArea(Vector3 position)
        {
            // エリア内の中心とpositionの差
            Vector3 diff = position - judgeArea.transform.position;
            // 差がsizeの半分以下ならbox内
            bool inX = Mathf.Abs(diff.x) <= judgeArea.size.x / 2;
            bool inY = Mathf.Abs(diff.y) <= judgeArea.size.y / 2;
            bool inZ = Mathf.Abs(diff.z) <= judgeArea.size.z / 2;

            return inX && inY && inZ;
        }
    }

    public interface IAreaChecker
    {
        bool IsInArea(Vector3 position);
    }
}
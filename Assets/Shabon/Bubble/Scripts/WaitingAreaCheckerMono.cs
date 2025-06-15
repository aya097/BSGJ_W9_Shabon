#nullable enable 

using UnityEngine;

namespace Shabon.Bubble
{
    /// <summary>
    /// 範囲内にオブジェクトがいるか判定
    /// </summary>
    public class WaitingAreaCheckerMono : MonoBehaviour, IAreaChecker
    {
        [SerializeField] SphereCollider judgeArea = null!;

        public bool IsInArea(Vector3 position)
        {
            // エリア内の中心とpositionの差
            Vector3 diff = position - judgeArea.transform.position;
            // 差がsizeの半分以下ならbox内
            // bool inX = Mathf.Abs(diff.x) <= judgeArea.size.x / 2;
            // bool inY = Mathf.Abs(diff.y) <= judgeArea.size.y / 2;
            // bool inZ = Mathf.Abs(diff.z) <= judgeArea.size.z / 2;

            // 差が半径以下なら中
            return diff.sqrMagnitude <= Mathf.Pow(judgeArea.radius, 2);
        }
    }

    public interface IAreaChecker
    {
        bool IsInArea(Vector3 position);
    }
}
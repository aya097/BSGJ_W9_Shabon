#nullable enable
using UnityEngine;

namespace Shabon.Ui
{
    [CreateAssetMenu(fileName = "DirtViewParam", menuName = "Scriptable Objects/DirtViewParam")]
    public class DirtViewParam : ScriptableObject, IDirtViewParam
    {
        [Header("汚れエフェクトのプレハブ")]
        [SerializeField] GameObject dirtPrefab = null!;

        // ゲッター
        public GameObject DirtPrefab
        {
            get { return dirtPrefab; }
        }
    }

    public interface IDirtViewParam
    {
        GameObject DirtPrefab { get; }
    }
}
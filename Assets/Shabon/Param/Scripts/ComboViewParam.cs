#nullable enable
using UnityEngine;

namespace Shabon.Ui
{
    [CreateAssetMenu(fileName = "ComboViewParam", menuName = "Scriptable Objects/ComboViewParam")]
    public class ComboViewParam : ScriptableObject, IComboViewParam
    {
        [Header("コンボのプレハブ")]
        [SerializeField] ComboViewMono comboPrefab = null!;

        // ゲッター
        public ComboViewMono ComboPrefab
        {
            get { return comboPrefab; }
        }
    }

    public interface IComboViewParam
    {
        ComboViewMono ComboPrefab { get; }
    }
}
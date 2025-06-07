#nullable enable
using UnityEngine;

namespace Shabon.Ui
{
    [CreateAssetMenu(fileName = "ComboViewParam", menuName = "Scriptable Objects/ComboViewParam")]
    public class ComboViewParam : ScriptableObject, IComboViewParam
    {
        [SerializeField] ComboViewMono comboPrefab;

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
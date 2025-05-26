#nullable enable
using UnityEngine;
using TMPro;
using VContainer;
using R3;
using Shabon.Bubble;

namespace Shabon.SelfDebug
{
    /// <summary>
    /// Comboをデバッグ用UIに表示するクラス
    /// </summary>
    public class DebugComboViewMono : MonoBehaviour
    {
        [SerializeField] TMP_Text comboText = null!;

        [Inject]
        public void Initialize(IBubbleCombo bubbleCombo)
        {
            Observable.EveryValueChanged(bubbleCombo, b => b.ComboNum)
            .Subscribe(value =>
            {
                comboText.text = $"Combo	: {value}";
            }).AddTo(this);
        }
    }
}

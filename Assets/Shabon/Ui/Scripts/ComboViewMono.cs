#nullable enable
using TMPro;
using UnityEngine;

namespace Shabon.Ui
{
    /// <summary>
    /// 画面に表示されるコンボの本体
    /// </summary>
    public class ComboViewMono : MonoBehaviour
    {
        [SerializeField] TMP_Text comboText = null!;    // コンボ数を記入するテキスト

        public void SetCombo(int comboNum)
        {
            comboText.text = $"COMBO {comboNum}";
        }
    }
}
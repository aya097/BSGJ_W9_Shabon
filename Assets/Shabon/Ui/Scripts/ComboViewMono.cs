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
        [SerializeField] TMP_Text comboEvaluationText = null!; // コンボの評価を表示する用のテキスト

        // コンボの情報を設定
        public void SetCombo(int comboNum, string evaluationText)
        {
            comboEvaluationText.text = evaluationText;
            comboText.text = $"COMBO {comboNum}";
        }

    }
}
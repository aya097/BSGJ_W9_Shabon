#nullable enable

using TMPro;
using UnityEngine;
using Shabon.Ui;


namespace Shabon.Ui
{
    /// <summary>
    /// 画面に表示されるコンボの本体
    /// </summary>
    public class ComboViewMono : MonoBehaviour
    {
        [SerializeField] TMP_Text comboText = null!;    // コンボ数を記入するテキスト
        [SerializeField] TMP_Text comboEvaluationText = null!; // コンボの評価を表示する用のテキスト
        [SerializeField] TMP_Text rawComboText = null!; // まんまcomboとかかれたテキスト

        // Instanciateして親オブジェクトのスケール反映された後にスケール0にする
        void Start()
        {
            transform.localScale = Vector3.zero;
        }

        // コンボの情報を設定
        public void SetCombo(int comboNum, ComboEvaluationGroup comboEvaluationGroup, bool isBossClapped)
        {
            comboEvaluationText.text = comboEvaluationGroup.ComboEvaluation.ToString() + '!';
            comboEvaluationText.color = comboEvaluationGroup.TextColor;
            if (isBossClapped)
            {
                comboText.text = "";
                rawComboText.text = "";
            }
            else
            {
                comboText.text = $"{comboNum}";
                rawComboText.text = "combo";
            }
        }

    }
}
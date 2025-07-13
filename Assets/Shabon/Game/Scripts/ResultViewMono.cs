#nullable enable
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Shabon.Utility;

namespace Shabon.Game
{
    /// <summary>
    /// リザルト画面でデータを表示するクラス
    /// </summary>
    public class ResultViewMono : MonoBehaviour
    {
        [SerializeField] private GameObject resultCanvas = null!;
        [SerializeField] TMP_Text scoreText = null!;
        [SerializeField] TMP_Text dirtText = null!;
        [SerializeField] TMP_Text comboText = null!;
        [SerializeField] TMP_Text clapCountText = null!;
        [SerializeField] TMP_Text dirtDecreaseCountText = null!;
        [SerializeField] TMP_Text breathTimeText = null!;
        [SerializeField] TMP_Text calorieText = null!;
        [SerializeField] TMP_Text bossBattleTimeText = null!;




        void Awake()
        {
            Close();
        }
        public void Open()
        {
            SetData();
            resultCanvas.SetActive(true);
        }
        public void Close()
        {
            resultCanvas.SetActive(false);
        }

        void SetData()
        {
            ResultData.LoadResults();

            // ResultDataからデータを取得してUIに表示
            scoreText.text = $"{ResultData.FinalScore}";
            dirtText.text = $"{ResultData.FinalDirt}";
            comboText.text = $"{ResultData.FinalCombo}コンボ";
            clapCountText.text = $"{ResultData.FinalClapCount}回";
            dirtDecreaseCountText.text = $"{ResultData.FinalDirtDecreaseCount}回";
            breathTimeText.text = $"{(int)ResultData.FinalBreathTime}秒";
            // calorieText.text = $"{ResultData.FinalBreathStrengthSum:F2}";
            bossBattleTimeText.text = $"{(int)ResultData.BossBattleTime}秒";
        }
    }
}

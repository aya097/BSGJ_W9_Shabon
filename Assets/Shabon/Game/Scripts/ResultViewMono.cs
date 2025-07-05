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
        [SerializeField] Button titleButton = null!;
        [SerializeField] TMP_Text dirtText = null!;
        [SerializeField] TMP_Text scoreText = null!;
        [SerializeField] TMP_Text comboText = null!;
        [SerializeField] TMP_Text clapCountText = null!;
        [SerializeField] TMP_Text dirtDecreaseCountText = null!;
        [SerializeField] TMP_Text breathTimeText = null!;
        [SerializeField] TMP_Text breathStrengthSumText = null!;
        [SerializeField] TMP_Text bossBattleTimeText = null!;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            ResultData.LoadResults();

            // ResultDataからデータを取得してUIに表示
            dirtText.text = $"Dirt: {ResultData.FinalDirt}";
            scoreText.text = $"Score: {ResultData.FinalScore}";
            comboText.text = $"Combo: {ResultData.FinalCombo}";
            clapCountText.text = $"ClapCount: {ResultData.FinalClapCount}";
            dirtDecreaseCountText.text = $"DirtDecreaseCount: {ResultData.FinalDirtDecreaseCount}";
            breathTimeText.text = $"BreathTime: {ResultData.FinalBreathTime:F2} sec";
            breathStrengthSumText.text = $"BreathStrengthIntegral: {ResultData.FinalBreathStrengthSum:F2}";
            bossBattleTimeText.text = $"BossBattleTime: {ResultData.BossBattleTime:F2} sec";

            titleButton.onClick.AddListener(() =>
            {
                SceneTransition.Transition(SceneName.TitleScene);
            });
        }


    }
}

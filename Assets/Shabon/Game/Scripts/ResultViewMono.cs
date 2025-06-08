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

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // ResultDataからデータを取得してUIに表示
            dirtText.text = $"Dirt: {ResultData.FinalDirt}";
            scoreText.text = $"Score: {ResultData.FinalScore}";
            comboText.text = $"Combo: {ResultData.FinalCombo}";

            titleButton.onClick.AddListener(() =>
            {
                SceneTransition.Transition(SceneName.TitleScene);
            });
        }


    }
}

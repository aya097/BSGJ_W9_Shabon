#nullable enable
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Shabon.Utility;
using LitMotion;
using LitMotion.Extensions;

namespace Shabon.Game
{
    /// <summary>
    /// リザルト画面でデータを表示するクラス
    /// </summary>
    public class ResultViewMono : MonoBehaviour
    {
        [Header("テキスト")]
        [SerializeField] private GameObject resultCanvas = null!;
        [SerializeField] private GameObject resultText = null!;
        [SerializeField] TMP_Text scoreText = null!;
        [SerializeField] TMP_Text dirtText = null!;
        [SerializeField] TMP_Text comboText = null!;
        [SerializeField] TMP_Text clapCountText = null!;
        [SerializeField] TMP_Text dirtDecreaseCountText = null!;
        [SerializeField] TMP_Text breathTimeText = null!;
        [SerializeField] TMP_Text calorieText = null!;
        [SerializeField] TMP_Text bossBattleTimeText = null!;

        [Header("ぼかしフィルタ")]
        [SerializeField] private SpriteRenderer filter = null!;

        [Header("遷移時間")]
        [SerializeField] private float filterTime = 1f;
        [SerializeField] private float downTime = 0.5f;
        [SerializeField] private Ease downEase;

        [Header("タペストリーのボスと屋敷")]
        [SerializeField] private GameObject winMansion = null!;
        [SerializeField] private GameObject loseMansion = null!;
        [SerializeField] private GameObject winBoss = null!;
        [SerializeField] private GameObject loseBoss = null!;


        private

        void Awake()
        {
            Close();
        }
        public void Open(GameState gameState)
        {
            SetData();

            if (gameState == GameState.Win)
            {
                winMansion.SetActive(true);
                loseMansion.SetActive(false);
                winBoss.SetActive(false);
                loseBoss.SetActive(true);
            }
            else if (gameState == GameState.Lose)
            {
                winMansion.SetActive(false);
                loseMansion.SetActive(true);
                winBoss.SetActive(true);
                loseBoss.SetActive(false);
            }
            else
            {
                winMansion.SetActive(false);
                loseMansion.SetActive(false);
                winBoss.SetActive(false);
                loseBoss.SetActive(false);
            }
            resultCanvas.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 1200f);
            resultCanvas.SetActive(true);
            // ぼかしをかける
            filter.gameObject.SetActive(true);
            LMotion.Create(0.1f, 2f, filterTime)
                .WithOnComplete(() =>
                {
                    // タペストリーを下ろす
                    LMotion.Create(1200f, 0f, downTime)
                        .WithEase(downEase)
                        .WithOnComplete(() => resultText.SetActive(true))   // テキスト表示
                        .BindToAnchoredPositionY(resultCanvas.GetComponent<RectTransform>())
                        .AddTo(this);
                })
                .Bind(value =>
                {
                    filter.material.SetFloat("_TexelInterval", value);
                }).
                AddTo(this);
        }
        public void Close()
        {
            resultCanvas.SetActive(false);
            resultText.SetActive(false);
            filter.gameObject.SetActive(false);
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

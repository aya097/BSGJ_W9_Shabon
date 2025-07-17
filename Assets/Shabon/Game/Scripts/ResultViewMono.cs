#nullable enable
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Shabon.Utility;
using LitMotion;
using LitMotion.Extensions;
using R3;
using System;

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
        [Header("ゲーム結果テキスト")]
        [SerializeField] TMP_Text winText = null!;
        [SerializeField] TMP_Text winBackText = null!;
        [SerializeField] TMP_Text loseText = null!;
        [SerializeField] TMP_Text loseBackText = null!;
        [SerializeField] private Ease fontEase;

        private float _fontSize;

        [Header("遷移時間")]
        [Header("ゲームオーバー")]
        [SerializeField] private float filterGameOverTime = 2f;
        [SerializeField] private float showingGameOverTextTime = 3f;
        [Header("ゲームクリア")]
        [SerializeField] private float filterClearTime = 1f;
        [SerializeField] private float showingClearTextTime = 3f;

        [SerializeField] private float downTime = 0.5f;
        [SerializeField] private Ease downEase;

        [Header("タペストリーのボスと屋敷")]
        [SerializeField] private GameObject winMansion = null!;
        [SerializeField] private GameObject loseMansion = null!;
        [SerializeField] private GameObject winBoss = null!;
        [SerializeField] private GameObject loseBoss = null!;

        [Header("Clapアイコン")]
        [SerializeField] private GameObject clapIcon = null!;


        void Awake()
        {
            Close();
            _fontSize = winText.fontSize;
        }
        public void Open(GameState gameState)
        {
            // ★ここで保存
            ResultData.SaveResults(
                ResultData.FinalDirt,
                ResultData.FinalScore,
                ResultData.FinalCombo,
                ResultData.FinalClapCount,
                ResultData.FinalDirtIncreaseCount,
                ResultData.FinalBreathTime,
                ResultData.FinalBreathStrengthSum,
                ResultData.BossBattleTime
            );
            Shabon.Score.RankingScore.SaveScore(ResultData.FinalScore);

            SetData();

            float filterTime = 0;
            float showingTime = 0;
            if (gameState == GameState.Win)
            {
                winMansion.SetActive(true);
                loseMansion.SetActive(false);
                winBoss.SetActive(false);
                loseBoss.SetActive(true);

                filterTime = filterClearTime;
                showingTime = showingClearTextTime;
            }
            else if (gameState == GameState.Lose)
            {
                winMansion.SetActive(false);
                loseMansion.SetActive(true);
                winBoss.SetActive(true);
                loseBoss.SetActive(false);

                filterTime = filterGameOverTime;
                showingTime = showingGameOverTextTime;
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
                .Bind(value =>
                {
                    filter.material.SetFloat("_TexelInterval", value);
                }).
                AddTo(this);

            // ぼかしと同時にフォントを表示
            if (gameState == GameState.Win)
            {
                winBackText.gameObject.SetActive(true);
                loseBackText.gameObject.SetActive(false);
                LMotion.Create(0f, 1f, filterTime)
                    .WithEase(fontEase)
                    .Bind(value =>
                    {
                        winBackText.alpha = value;
                        winText.alpha = value;
                        // フォントサイズも変える
                        winBackText.fontSize = _fontSize * (1.5f - value / 2);
                        winText.fontSize = _fontSize * (1.5f - value / 2);
                    })
                    .AddTo(this);
            }
            else if (gameState == GameState.Lose)
            {
                winBackText.gameObject.SetActive(false);
                loseBackText.gameObject.SetActive(true);
                LMotion.Create(0f, 1f, filterTime)
                    .WithEase(fontEase)
                    .Bind(value =>
                    {
                        loseBackText.alpha = value;
                        loseText.alpha = value;

                        loseBackText.fontSize = _fontSize * (1.5f - value / 2);
                        loseText.fontSize = _fontSize * (1.5f - value / 2);
                    })
                    .AddTo(this);
            }

            // タペストリー下ろす(テキストを見せ終わったら)
            Observable.Timer(TimeSpan.FromSeconds(filterTime + showingTime))
                .Subscribe(_ =>
                {
                    LMotion.Create(1200f, 0f, downTime)
                        .WithEase(downEase)
                        .WithOnComplete(() => resultText.SetActive(true))   // テキスト表示
                        .BindToAnchoredPositionY(resultCanvas.GetComponent<RectTransform>())
                        .AddTo(this);
                })
                .AddTo(this);
        }

        public void SetClap(bool isActive)
        {
            clapIcon.SetActive(isActive);
        }

        public void Close()
        {
            resultCanvas.SetActive(false);
            resultText.SetActive(false);
            filter.gameObject.SetActive(false);
            winBackText.gameObject.SetActive(false);
            loseBackText.gameObject.SetActive(false);
        }

        void SetData()
        {
            ResultData.LoadResults();

            // ResultDataからデータを取得してUIに表示
            scoreText.text = $"{ResultData.FinalScore}";
            dirtText.text = $"{ResultData.FinalDirt}";
            dirtDecreaseCountText.text = $"{ResultData.FinalDirtIncreaseCount}回";
            bossBattleTimeText.text = $"{ResultData.BossBattleTime:0.0}秒";
            comboText.text = $"{ResultData.FinalCombo}コンボ";
            clapCountText.text = $"{ResultData.FinalClapCount}回";
            breathTimeText.text = $"{ResultData.FinalBreathTime:0.0}秒";
            calorieText.text = $"{ResultData.FinalCalorie:0.0} kcal";
        }
    }
}

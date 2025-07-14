using System.Collections.Generic;
using UnityEngine;
using TMPro;
using R3;
using LitMotion;
using LitMotion.Extensions;

public class RankingView : MonoBehaviour
{
    [SerializeField] TMP_Text rankText = null!;

    void Start()
    {
        FixTextPosition(rankText, -1800f);
        rankText.alignment = TextAlignmentOptions.Center;

        var scores = Shabon.Score.RankingScore.LoadScores();
        string[] colors = { "#FFE066", "#FFFFFF", "#B87333" }; // 金・銀・銅
        int[] sizes = { 36, 36, 36 };

        int count = 3;
        string[] lines = new string[count];

        // 3位→2位→1位の順に遅延して表示
        for (int idx = count - 1; idx >= 0; idx--)
        {
            int rank = idx + 1;
            int score = (idx < scores.Count) ? scores[idx] : 0;
            int displayIdx = idx; // クロージャ用

            Observable.Timer(System.TimeSpan.FromSeconds(0.7 * (count - 1 - idx)))
                .Subscribe(_ =>
                {
                    LMotion.Create(0, score, 2.5f)
                        .WithEase(Ease.OutCubic)
                        .Bind(value =>
                        {
                            lines[displayIdx] = $"<color={colors[displayIdx]}><size={sizes[displayIdx]}>{rank}: {value:0}</size></color>";
                            rankText.text = string.Join("\n", lines);
                        })
                        .AddTo(this);
                })
                .AddTo(this);
        }
    }

    private void FixTextPosition(TMP_Text text, float yOffset)
    {
        if (text == null) return;
        var rt = text.rectTransform;
        rt.anchorMin = new Vector2(0.5f, 1f);   // 上中央
        rt.anchorMax = new Vector2(0.5f, 1f);   // 上中央
        rt.pivot = new Vector2(0.5f, 1f);       // 上中央
        rt.anchoredPosition = new Vector2(0, yOffset); // 上端中央から下にずらす
        text.alignment = TextAlignmentOptions.Center;   // 中央揃え


    }
}


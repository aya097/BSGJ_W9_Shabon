using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Shabon.Score;

public class RankingView : MonoBehaviour
{
    [SerializeField] TMP_Text NumberOneText = null!;
    [SerializeField] TMP_Text NumberTwoText = null!;
    [SerializeField] TMP_Text NumberThreeText = null!;

    void Start()
    {
        // テキストの左端揃えを設定
        NumberOneText.alignment = TextAlignmentOptions.Left;
        NumberTwoText.alignment = TextAlignmentOptions.Left;
        NumberThreeText.alignment = TextAlignmentOptions.Left;

        // JSONファイルからスコアを取得
        var scores = RankingScore.LoadScores();

        // 上位3位を取得
        NumberOneText.text = scores.Count > 0 ? $"1st: {scores[0]}" : "1st: -";
        NumberTwoText.text = scores.Count > 1 ? $"2nd: {scores[1]}" : "2nd: -";
        NumberThreeText.text = scores.Count > 2 ? $"3rd: {scores[2]}" : "3rd: -";
    }
}

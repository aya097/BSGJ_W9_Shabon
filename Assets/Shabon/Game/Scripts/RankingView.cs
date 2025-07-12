using System.Collections.Generic;
using UnityEngine;
using TMPro;
using LitMotion;
using R3;

public class RankingView : MonoBehaviour
{
    // ランキング表示用のText（最大3つ）
    [SerializeField] TMP_Text[] rankTexts = null!; // Top 3

    void Start()
    {
        // スコアリストを取得
        var scores = Shabon.Score.RankingScore.LoadScores();
        // 表示する件数
        int count = Mathf.Min(rankTexts.Length, 3);

        // 3位→2位→1位の順で表示
        for (int i = count - 1; i >= 0; i--)
        {
            int idx = i; // クロージャ対策

            // Textが未設定ならスキップ
            if (rankTexts[idx] == null) continue;
            // 最初は非表示
            rankTexts[idx].alpha = 0;

            if (idx < scores.Count)
            {
                int rank = idx + 1;
                int score = scores[idx];

                // 3位から順に遅延して表示
                Observable.Timer(System.TimeSpan.FromSeconds(0.7 * (count - 1 - idx)))
                    .Subscribe(_ =>
                    {
                        // スコアを0からカウントアップ表示（2.5秒）
                        LMotion.Create(0, score, 2.5f)
                            .WithEase(Ease.OutCubic)
                            .Bind(value =>
                            {
                                // スコアを表示
                                if (idx < rankTexts.Length && rankTexts[idx] != null)
                                    rankTexts[idx].text = $"{rank}: {value:0}";
                            })
                            .AddTo(this);

                        // フェードイン（0.5秒）
                        LMotion.Create(0f, 1f, 0.5f)
                            .Bind(a => rankTexts[idx].alpha = a)
                            .AddTo(this);
                    })
                    .AddTo(this);
            }
            else
            {
                // スコアがなければ "---" を表示
                rankTexts[idx].text = $"{idx + 1}: ---";
                rankTexts[idx].alpha = 1;
            }
        }
    }
}

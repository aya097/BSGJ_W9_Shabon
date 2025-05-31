using UnityEngine;
using UnityEngine.UI;

namespace Shabon.Game
{
    public static class ResultData
    {
        public static int FinalDirt { get; set; }
        public static int FinalScore { get; set; }
        public static int FinalCombo { get; set; }

        /// <summary>
        /// データを保存するメソッド
        /// </summary>
        public static void SaveResults(int dirt, int score, int combo)
        {
            FinalDirt = dirt;
            FinalScore = score;
            FinalCombo = combo;
        }

        /// <summary>
        /// データをログに出力するメソッド
        /// </summary>
        public static void LogResults()
        {
            Debug.Log($"Dirt: {FinalDirt}, Score: {FinalScore}, Combo: {FinalCombo}");
        }
    }
}

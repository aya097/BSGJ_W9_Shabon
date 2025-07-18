using System.Diagnostics;
using System.IO;
using Shabon.Game;
using TMPro;
using UnityEngine;

namespace Rnaking
{
    public class RankingPathTestMono : MonoBehaviour
    {
        [SerializeField] TMP_Text pathText = null!;

        void Awake()
        {
            var results = ResultData.LoadAllResults();
            string s = "";
            foreach (var result in results)
            {
                s += $"Dirt: {result.FinalDirt}, Score: {result.FinalScore}, Combo: {result.FinalCombo}, ClapCount: {result.FinalClapCount}, BreathTime: {result.FinalBreathTime}, Calorie: {result.Calorie}";
            }
            pathText.text = s;
        }
    }
}

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
            // pathText.text = $"{ResultData.GetPath()}";
        }
    }
}

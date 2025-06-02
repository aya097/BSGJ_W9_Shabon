#nullable enable

using UnityEngine;
using UnityEngine.UI;

namespace Shabon.Title
{
    /// <summary>
    /// タイトルシーンのビュークラス
    /// </summary>
    public class TitleViewMono : MonoBehaviour
    {
        [SerializeField] private Button startButton = null!;

        // ゲッター
        public Button StartButton => startButton;
    }
}

#nullable enable
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;

namespace Shabon.Title
{
    /// <summary>
    /// 背景を拡大したり、エフェクトかけたり
    /// </summary>
    public class SelectLanguageViewMono : MonoBehaviour
    {
        [SerializeField] private GameObject logoView = null!;
        [SerializeField] private GameObject startView = null!;
        [SerializeField] private GameObject filterView = null!;
        [SerializeField] private TMP_Text japaneseText = null!;
        [SerializeField] private TMP_Text englishText = null!;

        [SerializeField] private float awayTime;
        [SerializeField] private Ease awayEase;

        public void Open()
        {
            // ロゴをふっとばす
            var logoTransform = logoView.GetComponent<RectTransform>();
            LMotion.Create(logoTransform.anchoredPosition.y, 1000, awayTime)
                .WithEase(awayEase)
                .BindToAnchoredPositionY(logoTransform)
                .AddTo(this);
            // スタートをふっとばす
            var startTransform = startView.GetComponent<RectTransform>();
            LMotion.Create(startTransform.anchoredPosition.y, -1000, awayTime)
                .WithEase(awayEase)
                .BindToAnchoredPositionY(startTransform)
                .AddTo(this);
        }
    }
}
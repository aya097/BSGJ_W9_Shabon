#nullable enable
using System;
using LitMotion;
using LitMotion.Extensions;
using R3;
using TMPro;
using UnityEngine;

namespace Shabon.Ui
{
    /// <summary>
    /// Phaseの切り替わりを表現する
    /// </summary>
    public class PhaseViewMono : MonoBehaviour
    {
        [SerializeField] private GameObject view = null!;
        [SerializeField] private TMP_Text phaseText = null!;
        [SerializeField] private float duration = 1;
        [SerializeField] private Ease ease;

        public void SetPhase(string phaseStr)
        {
            if (phaseStr == "Final")
            {
                phaseText.text = "Final Phase";
            }
            else
            {
                phaseText.text = "Phase " + phaseStr;
            }
            LMotion.Create(1500f, 0f, duration)
                    .WithEase(ease)
                    .BindToAnchoredPosition3DY(view.GetComponent<RectTransform>());

            Observable.Timer(TimeSpan.FromSeconds(2))
                .Subscribe(_ =>
                {
                    LMotion.Create(0f, 1500f, duration)
                        .WithEase(ease)
                        .BindToAnchoredPosition3DY(view.GetComponent<RectTransform>());
                });

        }

    }
}

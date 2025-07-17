#nullable enable
using System;
using LitMotion;
using LitMotion.Extensions;
using R3;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Shabon.Ui
{
    /// <summary>
    /// Phaseの切り替わりを表現する
    /// </summary>
    public class PhaseViewMono : MonoBehaviour
    {
        [SerializeField] private GameObject textView = null!;
        [SerializeField] private TMP_Text phaseText = null!;
        [SerializeField] private float duration = 1;
        [SerializeField] private Ease ease;

        [Header("バブルの説明")]
        [SerializeField] private GameObject bubbleView = null!;
        [SerializeField] private GameObject quickBubble = null!;
        [SerializeField] private GameObject armorBubble = null!;


        public void SetPhase(string phaseStr)
        {
            int phase = 0;
            if (phaseStr == "Final")
            {
                phaseText.text = "Final Phase";
            }
            else
            {
                phaseText.text = "Phase " + phaseStr;
                int.TryParse(phaseStr, out phase);
            }
            LMotion.Create(1500f, 0f, duration)
                    .WithEase(ease)
                    .BindToAnchoredPosition3DY(textView.GetComponent<RectTransform>());

            Observable.Timer(TimeSpan.FromSeconds(2))
                .Subscribe(_ =>
                {
                    LMotion.Create(0f, 1500f, duration)
                    .WithOnComplete(() => SetBubble(phase))
                        .WithEase(ease)
                        .BindToAnchoredPosition3DY(textView.GetComponent<RectTransform>());
                });

        }

        private void SetBubble(int num)
        {

            if (num == 2)
            {
                quickBubble.SetActive(true);
            }
            else if (num == 3)
            {
                armorBubble.SetActive(true);
            }
            else
            {
                return;
            }
            Time.timeScale = 0f;
            Observable.Timer(TimeSpan.FromSeconds(5f), UnityTimeProvider.UpdateIgnoreTimeScale)
                .Subscribe(_ =>
                {
                    quickBubble.SetActive(false);
                    armorBubble.SetActive(false);
                    Time.timeScale = 1f;
                });
        }

    }
}

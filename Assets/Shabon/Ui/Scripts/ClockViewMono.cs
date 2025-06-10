#nullable enable
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Shabon.Game;

namespace Shabon.Ui
{
    public class ClockViewMono : MonoBehaviour
    {
        [SerializeField] RectTransform clockHand = null!;


        public void SetTime(float currentTime, float minTime, float maxTime)
        {
            float timeRange = maxTime - minTime;
            if (timeRange <= 0)
            {
                Debug.LogWarning($"時間の幅が0以下です。{timeRange}");
                return;
            }

            // 現在の時間を範囲内に正規化
            float timeRatio = Mathf.Clamp((currentTime - minTime) / timeRange, 0f, 1f);

            // 時計の背景画像のfillAmountを更新

            clockHand.rotation = Quaternion.Euler(0, 0, -360f * timeRatio);



        }
    }
}


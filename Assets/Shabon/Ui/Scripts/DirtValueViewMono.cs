#nullable enable
using UnityEngine;
using UnityEngine.UI;

namespace Shabon.Ui
{
    /// <summary>
    /// 汚れゲージのViewClassです。
    /// </summary>
    public class DirtValueViewMono : MonoBehaviour
    {
        [SerializeField] private GameObject view = null!;
        [SerializeField] private Material viewMaterial = null!;
        [SerializeField] private Image cleanImage = null!;
        private float _value; // 現在表示してる値

        /// <summary>
        /// 値を反映します
        /// </summary>
        /// <param name="currentValue">現在の値</param>
        /// <param name="minValue">最小値</param>
        /// <param name="maxValue">最大値</param>
        public void SetValue(float currentValue, float minValue, float maxValue)
        {
            float range = maxValue - minValue;
            if (range <= 0)
            {
                Debug.LogWarning($"汚れ値の幅が0以下です。${range}");
            }

            _value = currentValue / range;
            // fillImage.fillAmount = _value;
            viewMaterial.SetFloat("_Ratio", 1 - _value);

            cleanImage.fillAmount = 1 - _value;
        }

        public void Open()
        {
            view.SetActive(true);
        }
        public void Close()
        {
            view.SetActive(false);
        }
    }
}
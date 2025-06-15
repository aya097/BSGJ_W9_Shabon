#nullable enable
using UnityEngine;
using UnityEngine.UI;

namespace Shabon.Ui
{
    /// <summary>
    /// ClapのエフェクトやUIのViewクラス
    /// </summary>
    public class ClapUiViewMono : MonoBehaviour
    {
        [SerializeField] private Image _coolTime = null!;

        public void SetCoolTime(float min, float max, float num)
        {
            float range = max - min;
            if (range <= 0) return;

            _coolTime.fillAmount = num / range;
        }
    }
}
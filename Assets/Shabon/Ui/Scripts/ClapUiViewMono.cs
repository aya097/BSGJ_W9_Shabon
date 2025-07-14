#nullable enable
using NUnit.Framework;
using Shabon.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace Shabon.Ui
{
    /// <summary>
    /// ClapのエフェクトやUIのViewクラス
    /// </summary>
    public class ClapUiViewMono : MonoBehaviour
    {
        [SerializeField] private GameObject view = null!;
        [SerializeField] private Image _coolTime = null!;
        [SerializeField] private GameObject _backLight = null!;
        [SerializeField] private GameObject _frontLight = null!;

        void Awake()
        {
            _coolTime.fillAmount = 1f;
            _backLight.SetActive(true);
            _frontLight.SetActive(true);
        }

        public void SetCoolTime(float min, float max, float num)
        {
            float range = max - min;
            if (range <= 0) return;

            _coolTime.fillAmount = num / range;

            // クールタイムが終わったらAnimationを再生する
            if (num >= max)
            {
                _backLight.SetActive(true);
                _frontLight.SetActive(true);
                SoundPlayerMono.Instance?.PlaySe(SeTypeEnum.ClapFull);
            }
            // クールタイム中はAnimationを止める
            else
            {
                _backLight.SetActive(false);
                _frontLight.SetActive(false);
            }
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
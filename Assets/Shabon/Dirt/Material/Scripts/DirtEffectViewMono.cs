#nullable enable
using UnityEngine;

namespace Shabon.Dirt
{
    /// <summary>
    /// 汚れエフェクト用クラス
    /// </summary>
    public class DirtEffectViewMono : MonoBehaviour
    {
        public bool IsActive { get; private set; } = false;

        public void Enable()
        {
            IsActive = true;

            gameObject.SetActive(true);
        }

        public void Disable()
        {
            IsActive = false;

            gameObject.SetActive(false);
        }
    }
}
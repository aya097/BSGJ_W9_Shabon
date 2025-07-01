#nullable enable
using UnityEngine;
using UnityEngine.UI;

namespace Shabon.Dirt
{
    /// <summary>
    /// 汚れエフェクト用クラス
    /// </summary>
    public class DirtEffectViewMono : MonoBehaviour
    {
        [SerializeField] private GameObject dirtEffect = null!; // 汚れエフェクト
        public bool IsActive { get; private set; } = false;

        void Awake()
        {
            IsActive = false;
            dirtEffect.SetActive(false);
        }
        public void Enable()
        {
            IsActive = true;

            dirtEffect.SetActive(true);
            // マテリアルをコピーして、インスタンス化する
            var image = dirtEffect.GetComponent<Image>();
            image.material = new Material(image.material);
            image.material.SetFloat("_Seed", Random.Range(0.0f, 100f));
        }

        public void Disable()
        {
            IsActive = false;

            dirtEffect.SetActive(false);
        }
    }
}
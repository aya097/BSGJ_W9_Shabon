#nullable enable
using LitMotion;
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
        [SerializeField] private float disappearTime = 0.5f;
        public bool IsActive { get; private set; } = false;

        private Image image = null!;
        private MotionHandle _disappearMotion;

        void Awake()
        {
            image = dirtEffect.GetComponent<Image>();
            IsActive = false;
            dirtEffect.SetActive(false);
        }
        public void Enable()
        {
            IsActive = true;
            // モーション中であれば、停止
            _disappearMotion.TryCancel();

            dirtEffect.SetActive(true);

            // 色を戻す
            var col = image.color;
            col.a = 1;
            image.color = col;

            // マテリアルをコピーして、インスタンス化する
            image.material = new Material(image.material);
            image.material.SetFloat("_Seed", Random.Range(0.0f, 100f));
        }

        public void Disable()
        {
            Debug.Log("Disable bubble");
            IsActive = false;
            // 徐々に暗くする
            _disappearMotion = LMotion.Create(image.color.a, 0f, disappearTime)
                .WithOnComplete(() => dirtEffect.SetActive(false))
                .Bind(value =>
                {
                    var col = image.color;
                    col.a = value;
                    image.color = col;
                });
        }
    }
}
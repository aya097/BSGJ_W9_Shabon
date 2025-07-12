#nullable enable

using LitMotion;
using LitMotion.Extensions;
using R3;
using UnityEditor.Rendering;
using UnityEngine;
using VContainer;

namespace Shabon.Clap
{
    /// <summary>
    /// Clapのエフェクトを実装するクラス
    /// </summary>
    public class ClapViewMono : MonoBehaviour
    {
        [Header("画面のエフェクトについて")]
        [SerializeField] private bool waveEnable = true;
        [SerializeField] private SpriteRenderer waveEffect = null!;
        [SerializeField] private float maxRadius = 1f;
        [SerializeField] private float waveSpeed = 0.5f;

        [Header("輪っかについて")]
        [SerializeField] private bool ringEnable = true;

        [SerializeField] private GameObject ringObject = null!;
        [SerializeField] private Ease ringEase;
        [SerializeField] private float ringRadius;
        [SerializeField] private float ringDuration;

        [Header("光について")]
        [SerializeField] private bool lightEnable = true;
        [SerializeField] private GameObject lightObject = null!;
        [SerializeField] private Material lightMaterial = null!;
        [SerializeField] private Ease lightEase;
        [SerializeField] private float lightRadius;
        [SerializeField] private float lightDuration;

        [Inject]
        public void Initialize(ClapModel clapModel)
        {
            // クラップされたら再生
            Observable.EveryValueChanged(clapModel, c => c.IsClap)
                .Subscribe(isClap =>
                {
                    if (isClap)
                    {
                        Play();
                    }
                }).AddTo(this);
        }

        private void Play()
        {
            if (waveEnable)
            {
                // 0から半径（全体の大きさに対する割合）まで変化
                LMotion.Create(0f, 1f, waveSpeed)
                    .WithEase(Ease.Linear)
                    .WithOnComplete(() =>
                    {
                        SetWave(0f);
                        LMotion.Create(0.3f, 2f, waveSpeed * 1.7f)
                        .WithEase(Ease.Linear)
                        .WithOnComplete(() => SetWave(0f))
                        .Bind(value => SetWave(value * maxRadius))
                        .AddTo(this);
                    }
                    )
                    .Bind(value => SetWave(value * maxRadius))
                    .AddTo(this);
            }

            if (ringEnable)
            {
                // リングの半径
                LMotion.Create(0f, ringRadius, ringDuration)
                .WithEase(ringEase)
                .WithOnComplete(() => SetRing(0f))
                .Bind(value => SetRing(value))
                .AddTo(this);
                // 色変化
                var ringRenderer = ringObject.GetComponent<SpriteRenderer>();
                LMotion.Create(1f, 0f, ringDuration)
                    .WithEase(Ease.OutQuart)
                    .BindToColorA(ringRenderer)
                    .AddTo(this);
            }

            if (lightEnable)
            {
                // ライトの半径
                LMotion.Create(0f, lightRadius, lightDuration)
                .WithEase(lightEase)
                .WithOnComplete(() => SetLight(0f))
                .Bind(value => SetLight(value))
                .AddTo(this);
            }
        }

        private void SetWave(float radius)
        {
            if (radius == 0)
            {
                waveEffect.gameObject.SetActive(false);
            }
            else
            {
                waveEffect.gameObject.SetActive(true);
            }
            waveEffect.material.SetFloat("_Radius", radius);
        }

        private void SetRing(float radius)
        {
            ringObject.transform.localScale = Vector3.one * radius;
        }

        private void SetLight(float radius)
        {
            if (radius == 0)
            {
                lightObject.gameObject.SetActive(false);
            }
            else
            {
                lightObject.gameObject.SetActive(true);
            }
            var scale = lightObject.transform.localScale;
            scale.x = radius;
            scale.z = radius;
            lightObject.transform.localScale = scale;
            lightMaterial.SetFloat("_alpha", 1.1f - radius / lightRadius);
        }
    }
}
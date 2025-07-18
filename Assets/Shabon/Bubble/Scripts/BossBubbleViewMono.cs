#nullable enable

using UnityEngine;
using R3;
using System;
using LitMotion;
using LitMotion.Extensions;

namespace Shabon.Bubble
{
    public class BossBubbleViewMono : BubbleViewMono
    {
        [Header("スポーン時のエフェクト")]
        [SerializeField] protected GameObject spawnEffect = null!;

        [Header("装飾品関係")]
        [SerializeField] protected Animator _bubbleOrnamentAnimator = null!;
        [SerializeField] protected SpriteRenderer _ornamentSpriteRenderer = null!;

        private int _ornamentOriginalOrderInLayer;
        private bool isBreathed = false;

        protected override bool EnableFloatMotion => false;

        protected override void Awake()
        {
            base.Awake();
            _ornamentOriginalOrderInLayer = _ornamentSpriteRenderer.sortingOrder;
        }

        protected override void TurnOnHighlight()
        {
            _spriteRenderer.material.SetFloat("_HighLightFlag", 1f);
            _ornamentSpriteRenderer.material.SetFloat("_HighLightFlag", 1f);
        }
        protected override void TurnOffHighlight()
        {
            _spriteRenderer.material.SetFloat("_HighLightFlag", 0f);
            _ornamentSpriteRenderer.material.SetFloat("_HighLightFlag", 0f);
        }
        // メイド
        protected override void SetDarkness(float value)
        {
            _spriteRenderer.color = _originalColor - new Color(value, value, value, 0f);
            _ornamentSpriteRenderer.color = _originalColor - new Color(value, value, value, 0f);

        }

        public override void PlayBreath(IBubbleMono bubbleMono)
        {
            if (!isBreathed)
            {
                isBreathed = true;
                LMotion.Shake.Create(0f, 10f, 0.1f)
                    .WithOnComplete(() => isBreathed = false)
                    .BindToEulerAnglesZ(_ornamentSpriteRenderer.gameObject.transform)
                    .AddTo(this);

            }
        }

        public override void PlayAttack(Action? callback = null)
        {
            // Breathをリセット
            // _breathDisposable?.Dispose();

            Play(BubbleAnimationEnum.Attack);
            Observable.Timer(TimeSpan.FromSeconds(0.8f))
                .Subscribe(_ =>
                {
                    Play(BubbleAnimationEnum.Idle);
                    callback?.Invoke();
                }).AddTo(this);
        }

        public override void PlayClap(Action? callback = null, bool isDead = true)
        {
            // Breathをリセット
            //_breathDisposable?.Dispose();
            if (!isDead)
            {
                Play(BubbleAnimationEnum.Clap);
                Observable.Timer(TimeSpan.FromSeconds(0.51f))
                    .Subscribe(_ =>
                    {
                        Play(BubbleAnimationEnum.Idle);
                        callback?.Invoke();
                    }).AddTo(this);

                return;
            }

            Play(BubbleAnimationEnum.Down);
            Observable.Timer(TimeSpan.FromSeconds(2.8f))
                .Subscribe(_ =>
                {
                    callback?.Invoke();
                }).AddTo(this);
            shadow.SetActive(false);
        }

        public void PlaySpawn(Action? callback = null)
        {
            Play(BubbleAnimationEnum.Spawn);
            spawnEffect.SetActive(true);
            Observable.Timer(TimeSpan.FromSeconds(0.9f))
                .Subscribe(_ =>
                {
                    spawnEffect.SetActive(false);
                    Play(BubbleAnimationEnum.Idle);
                    callback?.Invoke();
                }).AddTo(this);
        }
        public override void SetSortingLayer(string sortingLayerName)
        {
            base.SetSortingLayer(sortingLayerName);
            _ornamentSpriteRenderer.sortingLayerName = sortingLayerName;
        }

        protected override void Play(BubbleAnimationEnum animation)
        {
            if (_currentAnimation != animation)
            {
                Debug.Log("アニメーション変化!! : " + animation);
                _currentAnimation = animation;
                _bubbleAnimator.SetTrigger(animation.ToString());
                Debug.Log(animation.ToString());
                if (animation == BubbleAnimationEnum.Down)
                {
                    Debug.Log("Boss撃破!!!!");
                    _ornamentSpriteRenderer.enabled = false;
                }
                else
                {
                    _bubbleOrnamentAnimator.SetTrigger(animation.ToString());
                }
            }
        }

        // animatorのボスHP変数にセットするメソッド
        public void SetBossHp(int bossHp)
        {
            _bubbleAnimator.SetInteger("BossHp", bossHp);
            _bubbleOrnamentAnimator.SetInteger("BossHp", bossHp);
        }

        protected override void Update()
        {
            // プレイヤーに近い(zが小さい)bubbleほど、手前に表示させるように
            _ornamentSpriteRenderer.sortingOrder = (int)(-transform.position.z * 10000) + _ornamentOriginalOrderInLayer;
            base.Update();
        }
    }
}

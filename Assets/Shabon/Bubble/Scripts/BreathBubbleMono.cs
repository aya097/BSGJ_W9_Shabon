#nullable enable

using UnityEngine;
using System;

namespace Shabon.Bubble
{
    /// <summary>
    /// 通常バブル（ボスバブルではない）の動きやイベントを管理するための派生クラス
    /// </summary>
    public class BreathBubbleMono : BubbleMono
    {
        public event Action? OnDead;
        public bool IsBreathing
        {
            get { return _isBreathing; }
            set { _isBreathing = value; }
        }

        private float _requiredBreathTime;
        private float _breathResetInterval;
        private float _breathTimer = 0f;
        private float lastBreathTime = 0f;
        private bool _isBreathing = false;

        protected override void Update()
        {
            base.Update();

            if (_isBreathing)
            {
                _breathTimer += Time.deltaTime;
                _isBreathing = false;
                lastBreathTime = Time.time;
            }
            else if (Time.time - lastBreathTime > _breathResetInterval)
            {
                _breathTimer = 0f;
            }

            if (_breathTimer >= _requiredBreathTime)
            {
                InvokeOnDead();
            }
        }

        public override void SetBuildParam(
            IBubbleMover bubbleMover,
            BubbleDeath bubbleDeath,
            IAreaChecker areaChecker,
            IBubbleData bubbleData,
            BubbleCluster bubbleCluster
        )
        {
            
            _requiredBreathTime = bubbleData.RequiredBreathTime;
            _breathResetInterval = bubbleData.BreathResetInterval;
            base.SetBuildParam(bubbleMover, bubbleDeath, areaChecker, bubbleData, bubbleCluster);
        }


        public void InvokeOnDead()
        {
            OnDead?.Invoke();
        }

    }
}


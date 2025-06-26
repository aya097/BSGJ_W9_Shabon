#nullable enable

using System;
using UnityEngine;
using VContainer;
using Shabon.Score;
using UnityEngine.Video;
using Unity.VisualScripting;

namespace Shabon.Bubble
{
    public class BossBubbleMono : BubbleMono
    {
        public bool IsBreathing
        {
            get { return _isBreathing; }
            set { _isBreathing = value; }
        }

        public int BossHp
        {
            get { return _bossHp; }
            set { _bossHp = value; }
        }
        public bool IsAttacked => _isAttacked;

        private bool _isBreathing = false;
        private bool _isAttacked = false;
        private int _bossHp = 0;
        private Vector3 _basePosition;


        void Start()
        {
            _basePosition = gameObject.transform.position;
        }

        protected override void Update()
        {

            // 停止中だったら動かない
            if (_isStop) return;

            if (_isAttacked)
            {
                if (_bubbleMover is BossBubbleMover bossBubbleMover)
                    bossBubbleMover.MoveBackward(_basePosition);
                int distance = (int)Vector3.Distance(transform.position, _basePosition);
                if (distance == 0) _isAttacked = false;
            }
            else
            {
                _bubbleMover.MoveForward();
            }

            // waitArea
            if (_waitAreaChecker.IsInArea(transform.position))
            {
                _isReached = true;
                InvokeOnReach();
            }
        }

        public override void SetBuildParam(IBubbleMover bubbleMover, BubbleDeath bubbleDeath, IAreaChecker areaChecker, IBubbleData bubbleData)
        {
            base.SetBuildParam(bubbleMover, bubbleDeath, areaChecker, bubbleData);

            if (bubbleData is BossBubbleData bossBubbleData)
            {
                _bossHp = bossBubbleData.Hp;
            }
        }
    
    }
}


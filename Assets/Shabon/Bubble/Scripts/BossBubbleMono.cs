#nullable enable

using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Shabon.Bubble
{
    public class BossBubbleMono : BubbleMono, IBubbleMono
    {
        public event Action<IEnumerable<BubbleType>>? OnSpawn;

        public int BossHitPoint => _bossHitPoint;
        private bool _isBack = false;
        private Vector3 _basePosition;
        private int _bossHitPoint;
        private float _bubbleSpawnInterval;
        private int _bubbleSpawnNum;
        private float _currentTime = 0;

        void Awake()
        {
            _basePosition = new Vector3(0, gameObject.transform.position.y, 0.9f);
        }

        protected override void Update()
        {
            // 到達したら動かない
            if (_isReached) return;

            // 停止中だったら動かない
            if (_isStop) return;

            // 後退
            if (_isBack)
            {
                _bubbleMover.MoveBackward(_basePosition);
                if (Vector3.Distance(transform.position, _basePosition) < 0.001f)
                    _isBack = false;
                return;
            }

            // 前進
            _bubbleMover.MoveForward();

            // スポーンの処理
            _currentTime += Time.deltaTime;
            if (_currentTime >= _bubbleSpawnInterval)
            {
                InvokeOnSpawn();
                _currentTime = 0;
            }

            // waitArea
            if (_waitAreaChecker.IsInArea(transform.position))
            {
                _isReached = true;
                InvokeOnReach();
            }
        }

        public override void SetBuildParam(
            IBubbleMover bubbleMover,
            IAreaChecker areaChecker,
            IBubbleData bubbleData,
            BubbleCluster bubbleCluster
        )
        {
            _bossHitPoint = bubbleData.BossHitPoint;
            _bubbleSpawnInterval = bubbleData.BubbleSpawnInterval;
            _bubbleSpawnNum = bubbleData.BubbleSpawnNum;

            base.SetBuildParam(bubbleMover, areaChecker, bubbleData, bubbleCluster);

        }

        public void Back()
        {
            _isBack = true;
        }

        // HPを減らすメソッド
        public void DecreaseHp(int damage)
        {
            _bossHitPoint -= damage;
        }

        public void InvokeOnSpawn()
        {
            // ランダムなバブルタイプ取得(Noneは除外)
            List<BubbleType> bubbleTypeList = new();
            BubbleType randomBubbleType;
            foreach (var i in Enumerable.Range(0, _bubbleSpawnNum))
            {
                do
                {
                    Array bubbleTypes = Enum.GetValues(typeof(BubbleType));
                    int randomIndex = UnityEngine.Random.Range(0, bubbleTypes.Length - 1);
                    randomBubbleType = (BubbleType)bubbleTypes.GetValue(randomIndex);

                } while (randomBubbleType == BubbleType.None);
                bubbleTypeList.Add(randomBubbleType);
            }

            OnSpawn?.Invoke(bubbleTypeList);
        }

    }
}


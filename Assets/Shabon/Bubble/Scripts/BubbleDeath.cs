#nullable enable

using System;

namespace Shabon.Bubble
{
    /// <summary>
    /// バブルの割れ方
    /// </summary>
    public enum BubbleDeathType
    {
        Clap,   // クラップによって
        Attack, // バブルが攻撃して
        Chain,  // コンボに巻き込まれて
        Mine,   // 地雷バブルにまきこまれる
    }

    /// <summary>
    /// バブルの割れる処理を行うクラス。
    /// </summary>
    public class BubbleDeath
    {
        private readonly IDeathProcess _bubbleDeath;    // バブルの具体的な割れる処理
        public BubbleDeath(BubbleType bubbleType, Action destroyBubble)
        {
            _bubbleDeath = GetDeathProcess(bubbleType, destroyBubble);
        }

        // バブルの割れ方に応じて処理を行う
        public void InvokeDeath(BubbleDeathType bubbleDeathType)
        {
            switch (bubbleDeathType)
            {
                case BubbleDeathType.Clap:
                    _bubbleDeath.Clap();
                    break;
                case BubbleDeathType.Attack:
                    _bubbleDeath.Attack();
                    break;
                case BubbleDeathType.Chain:
                    _bubbleDeath.Chain();
                    break;
                case BubbleDeathType.Mine:
                    _bubbleDeath.Mine();
                    break;
                default:
                    break;
            }

        }
        private IDeathProcess GetDeathProcess(BubbleType bubbleType, Action destroyBubble)
        {
            return bubbleType switch
            {
                BubbleType.Normal => new NormalBubbleDeath(destroyBubble),

                _ => new NormalBubbleDeath(destroyBubble)
            };
        }
    }

    /// <summary>
    /// バブルの割れる処理の方法を記述する
    /// </summary>
    public interface IDeathProcess
    {
        void Clap();
        void Attack();
        void Chain();
        void Mine();
    }

    public class NormalBubbleDeath : IDeathProcess
    {
        private readonly Action _destroyBubble;  // バブルを削除する処理
        public NormalBubbleDeath(Action destroy)
        {
            _destroyBubble = destroy;
        }

        public void Clap()
        {
            // スコア増やす and コンボ and 連鎖

            // destroy
            _destroyBubble.Invoke();
        }
        public void Attack()
        {
            // 汚れ値増やす

            // destroy
            _destroyBubble.Invoke();
        }
        public void Chain()
        {
            // スコア増やす and コンボ and 連鎖

        }
        public void Mine()
        {
            _destroyBubble.Invoke();
        }
    }


}
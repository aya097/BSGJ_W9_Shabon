#nullable enable

using System;
using Shabon.Score;

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
    /// バブルがやられるときに変更されるパラメータ
    /// </summary>
    public class DeathParams
    {
        public IScoreValue Score { get; private set; }
        public IDirtValue Dirt { get; private set; }
        public IBubbleCombo Combo { get; private set; }

        public DeathParams(IScoreValue scoreValue,
            IDirtValue dirtValue,
            IBubbleCombo bubbleCombo)
        {
            Score = scoreValue;
            Dirt = dirtValue;
            Combo = bubbleCombo;
        }
    }

    /// <summary>
    /// バブルの割れる処理を行うクラス。
    /// </summary>
    public class BubbleDeath
    {
        private readonly IDeathProcess _bubbleDeath;    // バブルの具体的な割れる処理
        public BubbleDeath(BubbleType bubbleType, DeathParams deathParams, Action destroyBubble)
        {
            _bubbleDeath = GetDeathProcess(bubbleType, destroyBubble, deathParams);
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
        private IDeathProcess GetDeathProcess(BubbleType bubbleType, Action destroyBubble, DeathParams deathParams)
        {
            return bubbleType switch
            {
                BubbleType.Normal => new NormalBubbleDeath(destroyBubble, deathParams),
                BubbleType.Armor => new NormalBubbleDeath(destroyBubble, deathParams), 

                _ => new NormalBubbleDeath(destroyBubble, deathParams)
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
        private readonly DeathParams _deathParams;


        public NormalBubbleDeath(Action destroy, DeathParams deathParams)
        {
            _destroyBubble = destroy;
            _deathParams = deathParams;
        }

        public void Clap()
        {
            // スコア増やす
            _deathParams.Score.Increase(100);   // todo 仮

            // DirtValueを減らす
            _deathParams.Dirt.Decrease(1); // Clapで1減らす

            // コンボリセット
            // _deathParams.Combo.Reset();
            _deathParams.Combo.Increase();

            // destroy
            _destroyBubble.Invoke();
        }
        public void Attack()
        {
            // 汚れ値増やす
            _deathParams.Dirt.Increase(1);      // todo 仮

            // destroy
            _destroyBubble.Invoke();
        }
        public void Chain()
        {
            // スコア増やす and コンボ and 連鎖
            _deathParams.Score.Increase(100);   // todo 仮

            // コンボ増加
            _deathParams.Combo.Increase();

            // destroy
            _destroyBubble.Invoke();
        }
        public void Mine()
        {
            _destroyBubble.Invoke();
        }
    }


}
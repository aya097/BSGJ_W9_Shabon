using UnityEngine;
using System;
using Shabon.Bubble;
using Shabon.Breath;
using System.Collections.Generic;

namespace Shabon.Bubble
{
    public class BubbleHandler : IBubbleHandler
    {
        private BubbleMono targetBubble; // 操作対象のバブルを保持するフィールド
        public float breathForce = 10f; // バブルを動かす際の力の大きさ
        public float clapRangeMin = -20f; // Clapアクションの有効範囲の最小値
        public float clapRangeMax = 20f; // Clapアクションの有効範囲の最大値
        private bool canControlBubble = false; // バブルを操作可能かどうかを示すフラグ

        public BubbleMono TargetBubble => targetBubble; // 操作対象のバブルを公開するプロパティ

        public BubbleHandler()
        {
            // Clapイベントが発生した際にClapメソッドを呼び出すよう登録
            Shabon.Clap.Clap.OnClap += Clap;
        }

        public void SetTargetBubble(BubbleMono bubble)
        {
            // 操作対象のバブルを設定
            targetBubble = bubble;
        }


        public void ApplyBreath(float amount, Vector2 direction)
        {
            // todo 実装
            // 範囲に入っているバブルを動かす
            // bubbles.Move(amount);
        }
        // ↓↓↓↓↓これのこと
        public void Breath(Vector2 direction, float amount)
        {
            // バブルを指定された方向と力で動かす
            if (canControlBubble && targetBubble != null)
            {
                Vector3 moveDirection = new Vector3(direction.x, 0f, 0f).normalized; // 水平方向の移動ベクトルを計算
                targetBubble.Move(moveDirection * amount * breathForce); // バブルを移動
            }
        }

        public void Clap()
        {
            // Clapアクションが有効範囲内で発生した場合、バブルを破壊
            if (targetBubble != null && IsBubbleInClapRange())
            {
                DestroyBubble(targetBubble);
            }
        }

        private bool IsBubbleInClapRange()
        {
            // バブルがClapアクションの有効範囲内にあるかを判定
            if (targetBubble == null) return false;
            return targetBubble.transform.position.x >= clapRangeMin && targetBubble.transform.position.x <= clapRangeMax;
        }

        private void DestroyBubble(BubbleMono bubble)
        {
            // バブルを破壊し、関連する状態をリセット
            if (bubble != null)
            {
                Debug.Log($"HideFlags: {bubble.gameObject.hideFlags}"); // デバッグ用ログ
                GameObject.Destroy(bubble.gameObject, 0.1f); // バブルのゲームオブジェクトを破壊

                // OnDeadイベントを発火
                TriggerOnDead(); // イベントを専用メソッド経由で発火

                targetBubble = null; // 操作対象をリセット
                canControlBubble = false; // バブル操作を無効化
            }
        }

        public void EnableBubbleControl()
        {
            // バブル操作を有効化
            canControlBubble = true;
        }

        public void DisableBubbleControl()
        {
            // バブル操作を無効化
            canControlBubble = false;
        }

        // イベントを発火するメソッド
        public void TriggerOnReach()
        {
            targetBubble?.InvokeOnReach(); // 専用メソッドを呼び出してイベントを発火
        }

        public void TriggerOnClap()
        {

            targetBubble?.InvokeOnClap(); // 専用メソッドを呼び出してイベントを発火

            Debug.Log("Clap called with amount: " + amount);
            // todo clapされたときに、範囲内のBubbleMonoのOnClapを呼ぶ関数

        }

        public void TriggerOnDead()
        {
            targetBubble?.InvokeOnDead(); // 専用メソッドを呼び出してイベントを発火
        }

        public void HandleInput()
        {
            if (targetBubble == null) return;

            // 十字キーで移動
            float horizontal = UnityEngine.Input.GetAxis("Horizontal"); // UnityEngine.Input を明示的に使用
            float vertical = UnityEngine.Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontal, 0, vertical);
            targetBubble.Move(direction);

            // Cキーで削除
            if (UnityEngine.Input.GetKeyDown(KeyCode.C))
            {
                Clap();
            }
        }
    }
}
using UnityEngine;
using Shabon.Bubble;

namespace Shabon.Breath
{
    public class Breath
    {
        public static event System.Action<Vector2, float> OnBreath; // Breathイベントを通知するためのデリゲート
        private IBubbleHandler _bubbleHandler; // バブル操作を管理するハンドラー

        public Breath(IBubbleHandler bubbleHandler)
        {
            // コンストラクタでバブルハンドラーを受け取る
            _bubbleHandler = bubbleHandler;
        }

        public void TryBreath(Vector2 direction, float amount)
        {
            // Breathイベントを発火し、バブルハンドラーのBreathメソッドを呼び出す
            OnBreath?.Invoke(direction, amount);
            _bubbleHandler.Breath(direction, amount);

            // バブルが移動した場合、OnReachイベントを発火
            if (_bubbleHandler.TargetBubble != null && _bubbleHandler.TargetBubble.transform.position.z <= 0)
            {
                _bubbleHandler.TriggerOnReach(); // BubbleHandler経由でOnReachイベントを発火
            }
        }
    }
}
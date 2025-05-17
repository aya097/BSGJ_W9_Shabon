using UnityEngine;

namespace Shabon.Bubble
{
    public interface IBubbleHandler
    {
        // バブルを指定された方向と力で動かすメソッド
        void Breath(Vector2 direction, float amount);

        // Clapアクションを実行するメソッド
        void Clap();

        // 操作対象のバブルを取得するプロパティ
        BubbleMono TargetBubble { get; }

        // イベントを発火するメソッド
        void TriggerOnReach();
        void TriggerOnClap();
        void TriggerOnDead();

        // 入力を処理するメソッド
        void HandleInput();
    }
}
#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Shabon.Game
{
    /// <summary>
    /// ゲームのフェーズを実行するクラス
    /// </summary>
    public class GameExecutor : ITickable
    {
        private readonly IGamePhases _gamePhases;

        private double _currentTime;    // 現在の時間

        private List<PhaseEvent> _eventList = new();

        public GameExecutor(IGamePhases gamePhases)
        {
            _gamePhases = gamePhases;

            // 初期化
            _currentTime = 0;
        }

        void ITickable.Tick()
        {
            _currentTime += Time.deltaTime;

            InvokeEvent();
        }

        // 呼び出し時間がきているイベントを呼び出す
        void InvokeEvent()
        {
            // 呼び出し時間がきているEventを取得
            var invokableEvents = _eventList.Where(e => e.EventTime >= _currentTime);

            // 空なら早期Return Count()を使うと遅い場合がある
            if (!invokableEvents.Any())
            {
                return;
            }
            // 時間順に並びかえ
            invokableEvents = invokableEvents.OrderBy(e => e.EventTime);

            foreach (PhaseEvent phaseEvent in invokableEvents)
            {
                // イベント実行
                phaseEvent.Event.Invoke();

                // 実行したものをリストから消去
                _eventList.Remove(phaseEvent);
            }
        }


        /// <summary>
        /// 時間に応じて発生させられるイベント
        /// </summary>
        private class PhaseEvent
        {
            public double EventTime { get; }    // 発生時間
            public Action Event { get; }    // 発生イベント

            public PhaseEvent(double time, Action event_)
            {
                EventTime = time;
                Event = event_;
            }
        }
    }


}
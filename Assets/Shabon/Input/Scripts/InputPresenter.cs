#nullable enable
using Shabon.Breath;
using Shabon.Bubble;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Shabon.Input
{
    /// <summary>
    /// Inputとゲームロジックを結びつけるクラス
    /// </summary>
    public class InputPresenter : ITickable
    {
        private readonly IInputManager _inputManager;
        private readonly BreathModel _breath;
        private readonly IBubbleHandler _bubbleHandler;

        [Inject]
        public InputPresenter(
            IInputManager inputManager,
            BreathModel breath,
            IBubbleHandler bubbleHandler
        )
        {
            _inputManager = inputManager;
            _breath = breath;
            _bubbleHandler = bubbleHandler;
        }

        float _ratio = 0f;
        void ITickable.Tick()
        {
            // Breath
            float horizontal = _inputManager.GetHorizontalDirection();
            float amount = _inputManager.GetBreath();
            _ratio += horizontal * 0.001f;  // 適当な値
            _ratio = Mathf.Max(-1, _ratio);
            _ratio = Mathf.Min(1, _ratio);

            _breath.SetDirection(_ratio);
            _breath.ApplyBreath(amount);

            // Clap
            if (_inputManager.GetClap()) // シフトキーが押されたとき
            {
                _bubbleHandler.ApplyClap(Vector3.zero, 1f);
            }
        }

    }
}
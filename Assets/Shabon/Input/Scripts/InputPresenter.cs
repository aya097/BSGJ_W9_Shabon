#nullable enable
using Shabon.Breath;
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

        [Inject]
        public InputPresenter(
            IInputManager inputManager,
            BreathModel breath
        )
        {
            _inputManager = inputManager;
            _breath = breath;
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
        }

    }
}
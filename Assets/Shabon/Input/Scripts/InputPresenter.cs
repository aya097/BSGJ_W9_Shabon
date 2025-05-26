#nullable enable
using Shabon.Breath;
using Shabon.Bubble;
using Shabon.Clap;
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
        private readonly ClapModel _clapModel;

        [Inject]
        public InputPresenter(
            IInputManager inputManager,
            BreathModel breath,
            IBubbleHandler bubbleHandler,
            ClapModel clapModel
        )
        {
            _inputManager = inputManager;
            _breath = breath;
            _bubbleHandler = bubbleHandler;
            _clapModel = clapModel;
        }

        float _ratio = 0f;
        void ITickable.Tick()
        {

            // Breath
            float horizontal = _inputManager.GetHorizontalDirection();
            float amount = _inputManager.GetBreath();
            _ratio += horizontal * 0.5f * Time.deltaTime;  // 適当な値
            _ratio = Mathf.Max(-1, _ratio);
            _ratio = Mathf.Min(1, _ratio);

            _breath.SetDirection(_ratio);
            _breath.ApplyBreath(amount);

            // Clap
            if (_inputManager.GetClap())
            {
                _clapModel.ApplyClap(1f);
            }
        }
    }
}
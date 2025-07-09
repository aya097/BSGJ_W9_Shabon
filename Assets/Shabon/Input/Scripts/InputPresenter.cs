#nullable enable
using Shabon.Breath;
using Shabon.Bubble;
using Shabon.Clap;
using Shabon.Game;
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
        private readonly IGameState _gameState;

        [Inject]
        public InputPresenter(
            IInputManager inputManager,
            BreathModel breath,
            IBubbleHandler bubbleHandler,
            ClapModel clapModel,
            IGameState gameState
        )
        {
            _inputManager = inputManager;
            _breath = breath;
            _bubbleHandler = bubbleHandler;
            _clapModel = clapModel;
            _gameState = gameState;
        }

        void ITickable.Tick()
        {
            if (_gameState.CurrentState == GameState.Tutorial)
            {

            }
            else if (_gameState.CurrentState == GameState.Game)
            {
                // Breath
                float _ratio = _inputManager.GetHorizontalDirection();
                float amount = _inputManager.GetBreath();
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
}
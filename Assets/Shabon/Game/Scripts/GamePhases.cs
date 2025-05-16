#nullable enable

namespace Shabon.Game
{
    public class GamePhases : IGamePhases
    {
        public int CurrentPhaseNum => _currentPhaseNum;
        private int _currentPhaseNum;

        public GamePhases()
        {

        }

        public GamePhaseData GetCurrentPhaseData()
        {
            return new GamePhaseData();
        }

        public void Proceed()
        {
            _currentPhaseNum++;
        }
    }
}
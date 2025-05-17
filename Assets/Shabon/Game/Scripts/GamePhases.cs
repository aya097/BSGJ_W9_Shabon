#nullable enable

using System.Linq;
using Shabon.Param;
using VContainer;

namespace Shabon.Game
{
    /// <summary>
    /// ゲームのフェーズを管理するクラス
    /// </summary>
    public class GamePhases : IGamePhases
    {
        public int CurrentPhaseNum => _currentPhaseNum; // 現在のフェーズ番号
        private int _currentPhaseNum;

        readonly IGamePhaseDataServer _gamePhaseDataServer;

        [Inject]
        public GamePhases(IGamePhaseDataServer gamePhaseDataServer)
        {
            _gamePhaseDataServer = gamePhaseDataServer;
            _currentPhaseNum = 0;
        }

        public GamePhaseData GetCurrentPhaseData()
        {
            return _gamePhaseDataServer.GetGamePhaseData().ElementAt(_currentPhaseNum); // 現在のフェーズ番号に適したデータを返す
        }

        public void Proceed()
        {
            _currentPhaseNum++;
        }
    }
}
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
        public int MaxPhaseNum => _gameRuleParam.GetGamePhaseDataList().Count();

        public int CurrentPhaseNum => _currentPhaseNum; // 現在のフェーズ番号
        private int _currentPhaseNum;

        readonly IGameRuleParam _gameRuleParam;

        [Inject]
        public GamePhases(IGameRuleParam gameRuleParam)
        {
            _gameRuleParam = gameRuleParam;
            _currentPhaseNum = 0;
        }

        public IGamePhaseData GetCurrentPhaseData()
        {
            return _gameRuleParam.GetGamePhaseDataList().ElementAt(_currentPhaseNum); // 現在のフェーズ番号に適したデータを返す
        }

        public bool Proceed()
        {
            // フェーズの最大をこえないとき
            if (_currentPhaseNum + 1 < _gameRuleParam.GetGamePhaseDataList().Count())
            {
                _currentPhaseNum++;
                return false;
            }
            return true;
        }
    }
}
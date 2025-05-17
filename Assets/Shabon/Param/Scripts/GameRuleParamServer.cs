#nullable enable

using System.Collections.Generic;
using VContainer;

namespace Shabon.Param
{
    /// <summary>
    /// GameRuleParamを提供するクラス
    /// </summary>
    public class GameRuleParamServer : IGamePhaseDataServer
    {
        readonly GameRuleParam _gameRuleParam;
        [Inject]
        public GameRuleParamServer(GameRuleParam gameRuleParam)
        {
            _gameRuleParam = gameRuleParam;
        }

        IEnumerable<GamePhaseData> IGamePhaseDataServer.GetGamePhaseData()
        {
            return _gameRuleParam.gamePhaseDataList;
        }
    }
}
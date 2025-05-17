using System.Collections;
using System.Collections.Generic;

namespace Shabon.Param
{
    /// <summary>
    /// ゲームのフェーズデータを提供するインターフェース
    /// </summary>
    public interface IGamePhaseDataServer
    {
        IEnumerable<GamePhaseData> GetGamePhaseData(); // ゲームのフェーズデータを取得する
    }
}
using Shabon.Param;

namespace Shabon.Game
{
    public interface IGamePhases
    {
        int MaxPhaseNum { get; }
        int CurrentPhaseNum { get; }    // 現在のフェーズ番号
        IGamePhaseData GetCurrentPhaseData(); // 現在のフェーズデータ
        bool Proceed(); // 次のフェーズに進める 
    }
}
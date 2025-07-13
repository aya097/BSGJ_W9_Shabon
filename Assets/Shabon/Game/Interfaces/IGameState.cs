namespace Shabon.Game
{
    public enum GameState
    {
        None,
        Tutorial,
        Game,
        Win,
        Lose,
    }

    /// <summary>
    /// 現在のStateを取得するための関数
    /// </summary>
    public interface IGameState
    {
        GameState CurrentState { get; }
    }
}
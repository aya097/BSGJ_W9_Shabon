namespace Shabon.Score
{
    public interface IScoreValue
    {
        int ScoreNum { get; }
        bool IsIncrease { get; }
        void Increase(int value);
        void Decrease(int value);
        void Set(int value);
    }
}
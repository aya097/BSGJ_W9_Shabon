namespace Shabon.Score
{
    public interface IScoreValue
    {
        int ScoreNum { get; }
        void Increase(int value);
        void Decrease(int value);
        void Set(int value);
    }
}
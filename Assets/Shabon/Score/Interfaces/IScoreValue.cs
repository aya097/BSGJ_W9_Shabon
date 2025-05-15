namespace Shabon.Score
{
    public interface IScoreValue
    {
        int ScoreNum { get; }
        void Increase(int value);
        void Decrease(int minusValue);
        void Set(int value);
    }
}
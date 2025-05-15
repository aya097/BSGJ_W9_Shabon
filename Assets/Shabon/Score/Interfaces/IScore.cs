namespace Shabon.Score
{
    public interface IScore
    {
        int ScoreNum { get; }
        void Increase(int value);
        void Decrease(int minusValue);
        void Set(int value);
    }
}
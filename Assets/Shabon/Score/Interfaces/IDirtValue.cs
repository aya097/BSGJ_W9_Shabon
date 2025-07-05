namespace Shabon.Score
{
    public interface IDirtValue
    {
        int DirtNum { get; }
        void Increase(int value);

        void Set(int value);
        int DecreaseCount { get; }
        int TotalIncrease { get; }
        int TotalDecrease { get; }
        int ClapDecreaseCount { get; }

        void DecreaseByClap(int value);
    }
}
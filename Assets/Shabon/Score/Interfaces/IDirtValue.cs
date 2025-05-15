namespace Shabon.Score
{
    public interface IDirtValue
    {
        int DirtNum { get; }
        void Increase(int value);
        void Decrease(int value);
        void Set(int value);
    }
}
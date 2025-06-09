namespace Shabon.Bubble
{
    public interface IBubbleChain
    {
        bool IsChaining { get; }
        void ExecuteBubbleChain(IBubbleMono targetBubbleMono, float chainRadius);
    }
}


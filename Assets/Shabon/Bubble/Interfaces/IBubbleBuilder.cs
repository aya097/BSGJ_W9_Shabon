namespace Shabon.Bubble
{
    public interface IBubbleBuilder
    {
        void Build(IBubbleBuildSetter bubbleSetter, IBubbleMono bubbleMono, IBubbleData bubbleData);
    }
}

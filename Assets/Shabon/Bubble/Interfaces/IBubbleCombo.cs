using System.Collections.Generic;

namespace Shabon.Bubble
{
    public interface IBubbleCombo
    {
        void AddComboCount(IBubbleMono bubbleMono);
        void RemoveChainedBubble(IBubbleMono bubbleMono);
        void AddRemainingChainBubble(IEnumerable<IBubbleMono> bubbleMonos);
    }
}


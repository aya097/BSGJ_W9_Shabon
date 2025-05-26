using System.Collections.Generic;

namespace Shabon.Bubble
{
    public interface IBubbleCombo
    {
        int ComboNum { get; }
        void AddComboCount(IBubbleMono bubbleMono);
        void RemoveChainedBubble(IBubbleMono bubbleMono);
        void AddRemainingChainBubble(IEnumerable<IBubbleMono> bubbleMonos);
    }
}


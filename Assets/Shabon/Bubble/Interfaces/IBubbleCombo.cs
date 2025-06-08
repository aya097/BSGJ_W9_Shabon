using System.Collections.Generic;

namespace Shabon.Bubble
{
    public interface IBubbleCombo
    {
        int ComboNum { get; }
        void Increase();
        void Reset();
    }
}


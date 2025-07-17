using System.Collections.Generic;

namespace Shabon.Bubble
{
    public interface IBubbleCombo
    {
        int ComboNum { get; }
        int MaxNum { get; }
        bool IsCombo { get; }
        bool IsBossClapped { get; }
        void Increase(bool isBossClapped=false);
        void Reset();
    }
}


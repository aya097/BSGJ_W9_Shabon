namespace Shabon.Input
{
    public interface IInputManager
    {
        bool GetClap();
        float GetBreath();
        float GetHorizontalDirection();
        bool GetMenuOpen();
        bool GetMenuConfirm();
        bool GetMenuBack();
        public float GetVolumeAdjustment();
    }
}
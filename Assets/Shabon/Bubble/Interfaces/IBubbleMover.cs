using UnityEngine;
namespace Shabon.Bubble
{
    public interface IBubbleMover
    {
        void MoveForward();
        void MoveByBreath(Vector3 direction);
    }
}
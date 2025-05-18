using UnityEngine;
namespace Shabon.Bubble
{
    public interface IBubbleMover
    {
        void MoveForward(float velocity);
        void MoveByBreath(Vector3 direction);
    }
}
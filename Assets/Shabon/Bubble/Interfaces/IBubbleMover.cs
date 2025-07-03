using UnityEngine;
namespace Shabon.Bubble
{
    public interface IBubbleMover
    {
        void MoveForward();
        void MoveByBreath(Vector3 direction);

        void UpdateSeparate();
        void AddForce(Vector3 force);

        // Boss
        void MoveBackward(Vector3 basePosition) { }

    }
}
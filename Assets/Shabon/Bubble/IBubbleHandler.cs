using UnityEngine;

namespace Shabon.Bubble
{
    public interface IBubbleHandler
    {
        void Breath(Vector2 direction, float amount);
        void Clap(float amount);
    }
}
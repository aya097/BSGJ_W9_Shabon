using Shabon.Bubble;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using VContainer;

namespace Shabon.Bubble
{
    public class BubbleHandler : MonoBehaviour, IBubbleHandler
    {

        private BubbleMover targetBubble;

        public float breathForce = 10f;
        public float clapRangeMin = -20f;
        public float clapRangeMax = 20f;

        public void Breath(Vector2 direction, float amount)
        {
            // X軸方向の移動のみを考慮
            Vector3 moveDirection = new Vector3(direction.x, 0f, 0f).normalized;


            if (targetBubble != null)
            {
                targetBubble.Move(moveDirection * amount * breathForce);
            }
        }

        public void Clap(float amount)
        {
            Debug.Log("Clap called with amount: " + amount);
        }

    }
}
using Shabon.Bubble;
using UnityEngine;

public class BossBubbleMover : NormalBubbleMover, IBubbleMover
{
    private readonly Transform _transform;  // 制御するBubbleのtransform
    private readonly Transform _targetTransform; // PlayerのTransform
    private readonly float _forwardVelocity;

    public BossBubbleMover(
        Transform transform,
        float forwardVelocity,
        Transform targetTransform)
        : base(transform, forwardVelocity, targetTransform)
    {
        _transform = transform;
        _forwardVelocity = forwardVelocity;
        _targetTransform = targetTransform;
    }

    public void MoveBackward(Vector3 basePosition)
    {
        Vector3 directionToTarget = basePosition - _transform.position;
        directionToTarget.y = 0;
        _transform.Translate(directionToTarget.normalized * _forwardVelocity * 10 * Time.deltaTime, Space.World);
        
    }
}

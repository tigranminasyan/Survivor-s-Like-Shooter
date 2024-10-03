using UnityEngine;

public class ChasingState : IEnemyState
{
    private Transform _transform;
    private Transform _target;
    private float _moveSpeed;

    public ChasingState(Transform transform, float moveSpeed)
    {
        _transform = transform;
        _moveSpeed = moveSpeed;
    }

    public void Enter()
    {
        // Logic for when the enemy enters the chasing state (e.g., play animation)
    }

    public void Update()
    {
        if (_target != null)
        {
            Vector3 direction = (_target.position - _transform.position).normalized;
            _transform.position += direction * _moveSpeed * Time.deltaTime;
        }
    }

    public void Exit()
    {
        // Logic for when the enemy exits the chasing state (e.g., stop animation)
    }

    public void SetChaseTarget(Transform target)
    {
        _target = target;
    }
}
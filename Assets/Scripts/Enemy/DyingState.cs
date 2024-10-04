using DG.Tweening;
using UnityEngine;
using System;

public class DyingState : IEnemyState
{
    private static readonly int DeadHash = Animator.StringToHash("Dead");

    public event Action EnemyKilledEvent;

    private Transform _enemyTransform;
    private Animator _animator;

    public DyingState(Transform enemyTransform, Animator animator)
    {
        _enemyTransform = enemyTransform;
        _animator = animator;
    }

    public void Enter()
    {
        _animator.SetBool(DeadHash, true);
        DOVirtual.DelayedCall(0.2f, () => 
        {
            EnemyKilledEvent?.Invoke();
            UnityEngine.Object.Destroy(_enemyTransform.gameObject);
        });
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
}
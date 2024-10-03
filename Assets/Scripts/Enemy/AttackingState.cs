using UnityEngine;

public class AttackingState : IEnemyState
{
    private static readonly int AttackHash = Animator.StringToHash("Hit");

    private Animator _animator;
    private PlayerController _playerController;
    private int _damage;

    private float _hitPeriod = 1.0f;
    private float _attackTimer;
    
    public AttackingState(Animator animator,
        PlayerController playerController,
        int damage)
    {
        _animator = animator;
        _playerController = playerController;
        _damage = damage;
    }

    public void Enter()
    {
        _attackTimer = _hitPeriod; 
    }

    public void Update()
    {
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= _hitPeriod)
        {
            Hit();
            _attackTimer = 0f;
        }
    }

    private void Hit()
    {
        _playerController.GetDamage(_damage);
        _animator.SetTrigger(AttackHash);
    }

    public void Exit()
    {
        _attackTimer = _hitPeriod;
    }
}
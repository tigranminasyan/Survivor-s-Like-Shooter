using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyController : MonoBehaviour
{
    private enum EnemyState
    {
        Chasing,
        Attacking,
        Dying
    }

    [SerializeField] private EnemyHealthController _enemyHealthController;
    [SerializeField] private Animator _animator;
    
    [SerializeField, ReadonlyField] private int _damage;
    [SerializeField, ReadonlyField] private float _moveSpeed;
    [SerializeField, ReadonlyField] private int _experienceToGrante;

    private IEnemyState _currentState;
    private PlayerController _playerController;
    private Transform _playerTransform;
    private Dictionary<EnemyState, IEnemyState> _states = new Dictionary<EnemyState, IEnemyState>();
    
    private void Start()
    {
        _enemyHealthController.OnEnemyDie += OnEnemyDie;
    }

    private void Update()
    {
        if (_currentState != null)
        {
            _currentState.Update();
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            SwitchToAttackingState();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            SwitchToChasingState();
        }
    }

    private void OnEnemyDie()
    {
        _playerController.AddExperience(_experienceToGrante);
        SwitchToDyingState();
    }

    [Inject]
    public void Construct(PlayerController player, EnemyConfiguration enemyConfiguration)
    {
        _playerController = player;
        _playerTransform = player.transform;
        Init(enemyConfiguration);
        InitializeStates();
        if (_states[EnemyState.Chasing] is ChasingState chasingState)
        {
            chasingState.SetChaseTarget(_playerTransform);
        }
        SwitchToChasingState(); 
    }

    private void Init(EnemyConfiguration enemyConfiguration)
    {
        _enemyHealthController.Init(enemyConfiguration.health);
        _damage = enemyConfiguration.damage;
        _moveSpeed = enemyConfiguration.speed;
        _experienceToGrante = enemyConfiguration.grantingExperience;
    }

    private void InitializeStates()
    {
        _states[EnemyState.Chasing] = new ChasingState(transform, _moveSpeed);
        _states[EnemyState.Attacking] = new AttackingState(_animator, _playerController, _damage);
        _states[EnemyState.Dying] = new DyingState(transform, _animator);
    }
    
    private void SwitchToChasingState()
    {
        SwitchState(EnemyState.Chasing);
    }

    private void SwitchToAttackingState()
    {
        SwitchState(EnemyState.Attacking);
    }

    private void SwitchToDyingState()
    {
        SwitchState(EnemyState.Dying);
    }

    private void SwitchState(EnemyState newState)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }

        if (_states.TryGetValue(newState, out IEnemyState newStateInstance))
        {
            _currentState = newStateInstance;
            _currentState.Enter();
        }
    }
    
    public bool TryGetDyingState(out DyingState dyingState)
    {
        if (_states.TryGetValue(EnemyState.Dying, out IEnemyState state) && state is DyingState dying)
        {
            dyingState = dying;
            return true;
        }

        dyingState = null;
        return false;
    }

    public void KillEnemy()
    {
        SwitchState(EnemyState.Dying);
    }
}

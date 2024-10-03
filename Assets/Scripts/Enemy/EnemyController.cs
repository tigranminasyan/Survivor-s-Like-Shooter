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
    [SerializeField, Range(1, 10)] private int _damage = 1;
    [SerializeField, Range(1.0f, 10.0f)] private float _moveSpeed = 2.0f;

    private IEnemyState _currentState;
    private PlayerController _playerController;
    private Transform _playerTransform;
    private Dictionary<EnemyState, IEnemyState> _states = new Dictionary<EnemyState, IEnemyState>();
    
    private void Awake()
    {

    }
    
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
        SwitchToDyingState();
    }

    [Inject]
    public void Construct(PlayerController player)
    {
        _playerController = player;
        _playerTransform = player.transform;
        InitializeStates();
        if (_states[EnemyState.Chasing] is ChasingState chasingState)
        {
            chasingState.SetChaseTarget(_playerTransform);
        }
        SwitchToChasingState(); 
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
}

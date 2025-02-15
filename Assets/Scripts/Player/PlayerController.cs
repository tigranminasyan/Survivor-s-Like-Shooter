using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action LevelCompletedEvent;
    public event Action LevelFailedEvent;
    public event Action<int> PlayerExperienceChangeEvent;
    public event Action<int> PlayerHealthChangeEvent;
    public event Action<int> KilledEnemyCountChangedEvent;

    [SerializeField] private PlayerHealthController _playerHealthController;
    [SerializeField] private PlayerMovementController _playerMovementController;
    [SerializeField] private EnemyDetector _enemyDetector;
    [SerializeField, ReadonlyField] private int _playerExperience;
    [SerializeField, ReadonlyField] private int _playerMaxExperience;
    [SerializeField, ReadonlyField] private Vector3 _initialPosition;
    [SerializeField, ReadonlyField] private int _killedEnemyCount = 0;

    private void Awake()
    {
        _initialPosition = transform.position;
    }

    public void Init(LevelConfiguration levelConfiguration)
    {
        _playerMaxExperience = levelConfiguration.levelMaxExperience;
        _playerHealthController.Init(levelConfiguration.playerMaxHealth);
        _playerMovementController.Init(levelConfiguration.playerSpeed);
        _enemyDetector.Init(levelConfiguration.enemyDetectRadius);
    }
    
    public void GetDamage(int damage)
    {
        _playerHealthController.GetDamage(damage);
    }
    
    public void AddExperience(int experience)
    {
        _playerExperience += experience;
        if (_playerExperience >= _playerMaxExperience)
        {
            LevelCompletedEvent?.Invoke();
        }
        PlayerExperienceChangeEvent?.Invoke(_playerExperience);
    }
    
    public void OnPlayerHealthChange(int health)
    {
        PlayerHealthChangeEvent?.Invoke(health);
    }
    
    public void OnPlayerDeath()
    {
        LevelFailedEvent?.Invoke();
    }
    
    public void OnEnemyKilled()
    {
        _killedEnemyCount++;
        KilledEnemyCountChangedEvent?.Invoke(_killedEnemyCount);
    }
    
    public void ResetPlayer()
    {
        _playerExperience = 0;
        _killedEnemyCount = 0;
        _playerHealthController.ResetHealth();
        transform.position = _initialPosition;
    }
}

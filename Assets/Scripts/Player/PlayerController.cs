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

    [SerializeField] private PlayerHealthController _playerHealthController;
    [SerializeField, ReadonlyField] private int _playerExperience;
    [SerializeField, ReadonlyField] private int _playerMaxExperience;

    public int PlayerMaxExperience => _playerMaxExperience;
    public int GetMaxHealth => _playerHealthController.GetMaxHealth;
    
    private void Awake()
    {
        _playerMaxExperience = 100;
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
}

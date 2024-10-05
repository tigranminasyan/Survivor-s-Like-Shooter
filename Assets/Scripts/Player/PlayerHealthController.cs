using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [SerializeField, ReadonlyField] private int _maxHealth;
    [SerializeField, ReadonlyField] private int _health;
    [SerializeField, ReadonlyField] private bool _isDead = false;

    public void Init(int maxHealth)
    {
        _maxHealth = maxHealth;
        _health = _maxHealth;
    }
    
    public void GetDamage(int damage)
    {
        if (!_isDead)
        {
            _health -= damage;
            if (_health <= 0)
            {
                _isDead = true;
                _playerController.OnPlayerDeath();
            }
            _playerController.OnPlayerHealthChange(_health);
        }
    }

    public void ResetHealth()
    {
        _health = _maxHealth;
        _isDead = false;
    }
}

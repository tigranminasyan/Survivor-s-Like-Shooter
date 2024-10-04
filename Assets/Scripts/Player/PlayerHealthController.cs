using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [SerializeField, Range(1, 25)] private int _maxHealth = 10;
    [SerializeField, ReadonlyField] private int _health = 1;
    [SerializeField, ReadonlyField] private bool _isDead = false;

    private void Start()
    {
        _health = _maxHealth;
        _playerController.OnPlayerHealthChange(_health, true);
    }

    public void GetDamage(int damage)
    {
        if (!_isDead)
        {
            _health -= damage;
            if (_health <= 0)
            {
                _isDead = true;
            }
            _playerController.OnPlayerHealthChange(_health, false);
        }
    }
}

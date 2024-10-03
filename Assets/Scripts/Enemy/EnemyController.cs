using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int _health = 1;
    [SerializeField] ChasePlayer _chasePlayer;

    private Transform _playerTransform;

    public void GetDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    [Inject]
    public void Construct(PlayerMovementController player)
    {
        _playerTransform = player.transform;
        _chasePlayer.SetChaseTarget(_playerTransform);
    }
}

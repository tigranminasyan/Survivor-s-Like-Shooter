using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerGunController : MonoBehaviour
{
    [SerializeField] private Transform _gun;
    [SerializeField] private SpriteRenderer _gunSpriteRenderer;
    [SerializeField] private Transform _leftHand;
    [SerializeField] private Transform _rightHand;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private BulletController _bulletController;
    [SerializeField] private float _shootingInterval = 0.5f;
    [SerializeField] private EnemyDetector _enemyDetector;

    [SerializeField, ReadonlyField] private float _directionX;
    [SerializeField, ReadonlyField] private int _initalSortingOrder;

    [Inject]
    private GameManager _gameManager;
    
    private float _shootingTimer;

    private void Awake()
    {
        _shootingTimer = _shootingInterval;
        _initalSortingOrder = _gunSpriteRenderer.sortingOrder;
    }

    private void Update()
    {
        if (_gameManager.IsGameStarted)
        {
            _shootingTimer += Time.deltaTime;
            if (_shootingTimer >= _shootingInterval)
            {
                HandleShooting();
            }
        }
    }

    private void HandleShooting()
    {
        List<Transform> nearestReachableEnemy = _enemyDetector.GetNearestReachableEnemy();
        if (nearestReachableEnemy.Count > 0)
        {
            Transform target = nearestReachableEnemy[0];
            Vector3 direction = (target.position - _gun.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _directionX = direction.x;
            
            if (Mathf.Abs(_directionX) > 0.2f)
            {
                bool isLeft = _directionX < 0;
                _gunSpriteRenderer.sortingOrder = isLeft ? _initalSortingOrder + 1 : _initalSortingOrder;
                _gunSpriteRenderer.flipY = isLeft;
                _gun.position = isLeft ? _rightHand.position : _leftHand.position;
            }
            
            _gun.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            Shoot();
        }
    }

    private void Shoot()
    {
        _shootingTimer = 0f;
        Instantiate(_bulletController, _firePoint.position, _gun.rotation);
    }
}

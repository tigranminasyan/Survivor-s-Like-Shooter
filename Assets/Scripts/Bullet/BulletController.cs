using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField, Range(1, 10)] private float _speed = 5.0f;
    [SerializeField, Range(0.0f, 5)] private float _lifeTime = 2.0f;
    [SerializeField, Range(1, 10)] private int _damage = 1;

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
        _rigidbody.velocity = transform.right * _speed;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyHealthController enemy;
        collision.gameObject.TryGetComponent<EnemyHealthController>(out enemy);
        if (enemy != null)
        {
            enemy.GetDamage(_damage);
        }
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int _health = 1;

    public void GetDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}

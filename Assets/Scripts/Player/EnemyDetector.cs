using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField, ReadonlyField] private float _detectRadius;
    [SerializeField] private LayerMask _enemyLayer;
    
    public void Init(float detectRadius)
    {
        _detectRadius = detectRadius;
    }

    public List<Transform> GetNearestReachableEnemy()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _detectRadius, _enemyLayer);
        List<Transform> enemies = new List<Transform>();

        float nearestDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            Transform enemy = hitCollider.transform;
            float distance = Vector2.Distance(transform.position, enemy.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                if (enemies.Count > 0)
                {
                    enemies[0] = enemy;
                }
                else
                {
                    enemies.Add(enemy);
                }
            }
        }

        return enemies;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectRadius);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform _enemiesParent;
    
    [Inject]
    private PlayerController _player;
    
    private void Start()
    {
        _player.LevelCompletedEvent += OnLevelComplete;
        _player.LevelFailedEvent += OnLevelFailed;
    }
    
    private void OnLevelComplete()
    {
        KillAllEnemies();
    }

    private void OnLevelFailed()
    {
        KillAllEnemies();
    }
    
    private void KillAllEnemies()
    {
        foreach (Transform enemy in _enemiesParent)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            bool ieEnemy = enemyController != null;
            if (ieEnemy)
            {
                enemyController.KillEnemy();
            }
        }
    }
    
    private void OnDestroy()
    {
        _player.LevelCompletedEvent -= OnLevelComplete;
        _player.LevelFailedEvent -= OnLevelFailed;
    }
}

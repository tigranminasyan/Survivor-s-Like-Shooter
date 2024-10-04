using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class EnemySpawnController : MonoBehaviour
{
    public event Action<int> KilledEnemyCountChangedEvent;
    
    [SerializeField] private List<EnemyController> _enemyTypes;
    [SerializeField] private float _spawnPeriod = 1.0f;
    [SerializeField] private float _offset = 1.0f; 
    [SerializeField] private Transform _enemyContainer;
    [SerializeField] private bool _spawning;

    [SerializeField, ReadonlyField] private int _killedEnemyCount = 0;
    private Camera _playerCamera;

    [Inject]
    private PlayerController _player;
    private void Start()
    {
        _playerCamera = Camera.main;
        if (_playerCamera == null)
        {
            Debug.LogError("Main camera not found");
            _spawning = false;
        }
        StartCoroutine(SpawnEnemy());
    }
    
    private IEnumerator SpawnEnemy()
    {
        while (_spawning)
        {
            Vector3 spawnPosition = GetRandomPositionOutOfCameraView();
            EnemyController randomEnemy = _enemyTypes[Random.Range(0, _enemyTypes.Count)];
            EnemyController enemy = Instantiate(randomEnemy,
                spawnPosition,
                Quaternion.identity,
                _enemyContainer.transform);
            enemy.Construct(_player);
            
            if (enemy.TryGetDyingState(out DyingState dyingState))
            {
                dyingState.EnemyKilledEvent += OnEnemyKilled;
            }
            
            yield return new WaitForSeconds(_spawnPeriod);
        }
    }
    
    private Vector3 GetRandomPositionOutOfCameraView()
    {
        Vector3 screenBottomLeft = _playerCamera.ViewportToWorldPoint(Vector2.zero);
        Vector3 screenTopRight = _playerCamera.ViewportToWorldPoint(Vector2.one);

        int side = Random.Range(0, 4);
        Vector3 randomPosition = Vector2.zero;

        switch (side)
        {
            case 0:
                randomPosition = new Vector2(screenBottomLeft.x - _offset, Random.Range(screenBottomLeft.y, screenTopRight.y));
                break;
            case 1:
                randomPosition = new Vector2(screenTopRight.x + _offset, Random.Range(screenBottomLeft.y, screenTopRight.y));
                break;
            case 2:
                randomPosition = new Vector2(Random.Range(screenBottomLeft.x, screenTopRight.x), screenBottomLeft.y - _offset);
                break;
            case 3:
                randomPosition = new Vector2(Random.Range(screenBottomLeft.x, screenTopRight.x), screenTopRight.y + _offset);
                break;
        }

        return randomPosition;
    }
    
    public void StopSpawning()
    {
        _spawning = false;
    }
    
    private void OnEnemyKilled(GameObject enemyObject)
    {
        EnemyController enemy = enemyObject.GetComponent<EnemyController>();

        if (enemy.TryGetDyingState(out DyingState dyingState))
        {
            dyingState.EnemyKilledEvent -= OnEnemyKilled;
        }

        Destroy(enemyObject);
        _killedEnemyCount++;
        KilledEnemyCountChangedEvent?.Invoke(_killedEnemyCount);
    }
}

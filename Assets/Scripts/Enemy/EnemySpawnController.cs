using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class EnemySpawnController : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private float _offset = 1.0f; 
    [SerializeField] private Transform _enemyContainer;
    [SerializeField] private bool _spawning;

    [SerializeField, ReadonlyField] private List<EnemyConfiguration> _enemyConfigurationContainerer;
    [SerializeField, ReadonlyField] private float _spawnPeriod;

    [Inject] private PlayerController _player;
    [Inject] private GameManager _gameManager;

    private Camera _playerCamera;
    private float _spawnTimer;

    private void Start()
    {
        _playerCamera = Camera.main;
        if (_playerCamera == null)
        {
            Debug.LogError("Main camera not found");
            _spawning = false;
        }
        
        _levelManager.LevelLoadedEvent += OnLevelLoaded;
        Init();
    }

    private void Init()
    {
        LevelConfiguration levelConfiguration = _levelManager.GetCurrentLevelConfiguration();
        _spawnPeriod = levelConfiguration.enemySpawnPeriod;
        _spawnTimer = _spawnPeriod;
        _enemyConfigurationContainerer = levelConfiguration.enemyConfigurationContainerer;
    }

    public void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawning &&
            _spawnTimer >= _spawnPeriod &&
            _gameManager.IsGameStarted)
        {
            SpawnEnemy();
            _spawnTimer = 0f;
        }
    }

    private void OnLevelLoaded()
    {
        Init();
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomPositionOutOfCameraView();
        EnemyConfiguration randomEnemyConfiguration = _enemyConfigurationContainerer[Random.Range(0, _enemyConfigurationContainerer.Count)];
        EnemyController randomEnemy = randomEnemyConfiguration.prefab;
        EnemyController enemy = Instantiate(randomEnemy,
            spawnPosition,
            Quaternion.identity,
            _enemyContainer.transform);
        enemy.Construct(_player, randomEnemyConfiguration);

        if (enemy.TryGetDyingState(out DyingState dyingState))
        {
            dyingState.EnemyKilledEvent += OnEnemyKilled;
        }
    }
    
    private Vector3 GetRandomPositionOutOfCameraView()
    {
        Vector3 screenBottomLeft = _playerCamera.ViewportToWorldPoint(Vector2.zero);
        Vector3 screenTopRight = _playerCamera.ViewportToWorldPoint(Vector2.one);

        float randomX, randomY;

        switch (Random.Range(0, 4))
        {
            case 0: // Left
                randomX = screenBottomLeft.x - _offset;
                randomY = Random.Range(screenBottomLeft.y, screenTopRight.y);
                break;
            case 1: // Right
                randomX = screenTopRight.x + _offset;
                randomY = Random.Range(screenBottomLeft.y, screenTopRight.y);
                break;
            case 2: // Bottom
                randomX = Random.Range(screenBottomLeft.x, screenTopRight.x);
                randomY = screenBottomLeft.y - _offset;
                break;
            case 3: // Top
                randomX = Random.Range(screenBottomLeft.x, screenTopRight.x);
                randomY = screenTopRight.y + _offset;
                break;
            default:
                randomX = 0;
                randomY = 0;
                break;
        }

        Vector3 randomPosition = new Vector3(randomX, randomY, 0);
        
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
        _player.OnEnemyKilled();
    }
    
    private void OnDestroy()
    {
        _levelManager.LevelLoadedEvent -= OnLevelLoaded;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class EnemySpawnController : MonoBehaviour
{
    private struct EnemyProperties
    {
        public int health;
        public int damage;
        public float speed;
        public int grantingExperience;
    }
    
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private float _offset = 1.0f; 
    [SerializeField] private Transform _enemyContainer;
    [SerializeField] private bool _spawning;

    [SerializeField, ReadonlyField] private List<EnemyController> _enemyTypes;
    [SerializeField, ReadonlyField] private float _spawnPeriod;

    [Inject] private PlayerController _player;
    [Inject] private GameManager _gameManager;

    private Dictionary<EnemyController, EnemyProperties> _enemyTypesToConfiguration;
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
        InitialiseEnemyTypes(levelConfiguration.enemies);
    }
    
    private void InitialiseEnemyTypes(List<EnemyConfiguration> enemies)
    {
        _enemyTypes = new List<EnemyController>();
        _enemyTypesToConfiguration = new Dictionary<EnemyController, EnemyProperties>();
        foreach (EnemyConfiguration enemy in enemies)
        {
            EnemyController enemyController = enemy.prefab;
            _enemyTypes.Add(enemyController);
            
            EnemyProperties enemyProperties = new EnemyProperties
            {
                health = enemy.health,
                damage = enemy.damage,
                speed = enemy.speed,
                grantingExperience = enemy.grantingExperience
            };
            _enemyTypesToConfiguration[enemy.prefab] = enemyProperties;
        }
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
        
        EnemyProperties enemyProperties = _enemyTypesToConfiguration[randomEnemy];
        enemy.Init(enemyProperties.health,
            enemyProperties.damage,
            enemyProperties.speed,
            enemyProperties.grantingExperience);
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
        _player.OnEnemyKilled();
    }
    
    private void OnDestroy()
    {
        _levelManager.LevelLoadedEvent -= OnLevelLoaded;
    }
}

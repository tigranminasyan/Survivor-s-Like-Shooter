using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public event Action LevelLoadedEvent;

    [SerializeField] LevelConfiguration[] _levels;
    
    [SerializeField, ReadonlyField] private int _currentLevelIndex = 0;
    [SerializeField, ReadonlyField] private int _totalLevelCount;
    [SerializeField, ReadonlyField] private LevelConfiguration _currentLevel;

    public int GetCurrentLevelNumber => _currentLevelIndex + 1;

    private void Awake()
    {
        _totalLevelCount = _levels.Length;

        LoadCurrentLevel();
    }
    
    public LevelConfiguration GetCurrentLevelConfiguration()
    {
        return _currentLevel;
    }

    public void LoadCurrentLevel()
    {
        LoadLevel(_currentLevelIndex);
    }
    
    public void LoadNextLevel()
    {
        _currentLevelIndex++;

        int levelIndexToLoad = GetLeveIndexToLoad();
        LoadLevel(levelIndexToLoad);
    }

    private void LoadLevel(int index)
    {
        _currentLevel = _levels[index];
        LevelLoadedEvent?.Invoke();
    }
    
    private int GetLeveIndexToLoad()
    {
        int leveIndexToLoad = _currentLevelIndex;
        if (leveIndexToLoad >= _totalLevelCount)
        {
            leveIndexToLoad = Random.Range(0, _totalLevelCount-1);
        }
        
        return leveIndexToLoad;
    }
}

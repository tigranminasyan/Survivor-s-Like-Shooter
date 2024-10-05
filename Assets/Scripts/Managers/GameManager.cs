using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIController _uiController;
    [SerializeField] private LevelManager _levelManager;

    [Inject] private PlayerController _player;

    public bool IsGameStarted { get; private set; } 
    public int GetCurrentLevelNumber => _levelManager.GetCurrentLevelNumber;
    
    private void Awake()
    {
        _levelManager.LoadCurrentLevel();
        OnGameStart();
    }
    
    private void Start()
    {
        _player.LevelCompletedEvent += OnLevelComplete;
        _player.LevelFailedEvent += OnLevelFailed;
        _uiController.NextLevelBtnClickedEvent += OnLoadNextLevel;
        _uiController.ReplayBtnClickedEvent += OnReplay;
    }
    
    private void OnLevelComplete()
    {
        IsGameStarted = false;
    }

    private void OnLevelFailed()
    {
        IsGameStarted = false;
    }
    
    private void OnLoadNextLevel()
    {
        ResetPlayer();
        _levelManager.LoadNextLevel();
        OnGameStart();
        _uiController.UpdateLevelText();
    }

    private void OnReplay()
    {
        ResetPlayer();
        OnGameStart();
    }
    
    private void OnGameStart()
    {
        IsGameStarted = true;
    }

    private void OnDestroy()
    {
        _player.LevelCompletedEvent -= OnLevelComplete;
        _player.LevelFailedEvent -= OnLevelFailed;
        _uiController.NextLevelBtnClickedEvent -= OnLoadNextLevel;
        _uiController.ReplayBtnClickedEvent -= OnReplay;
    }
    
    private void ResetPlayer()
    {
        _player.ResetPlayer();
    }
}

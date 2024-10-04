using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject]
    private PlayerController _player;
    public bool IsGameStarted { get; private set; }

    private void Awake()
    {
        OnGameStart();
    }
    
    private void Start()
    {
        _player.LevelCompletedEvent += OnLevelComplete;
        _player.LevelFailedEvent += OnLevelFailed;
    }
    
    private void OnLevelComplete()
    {
        IsGameStarted = false;
    }

    private void OnLevelFailed()
    {
        IsGameStarted = false;
    }
    
    private void OnGameStart()
    {
        IsGameStarted = true;
    }

    private void OnDestroy()
    {
        _player.LevelCompletedEvent -= OnLevelComplete;
        _player.LevelFailedEvent -= OnLevelFailed;
    }
}

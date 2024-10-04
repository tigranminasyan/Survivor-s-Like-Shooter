using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIController : MonoBehaviour
{
    [SerializeField] private EnemySpawnController _enemySpawnController;
    [SerializeField] private Text _killedEnemiesText;
    [SerializeField] private Slider _playerExperienceSlider;
    [SerializeField] private Slider _playerHealthSlider;

    [Inject]
    private PlayerController _player;
    
    private void Start()
    {
        _killedEnemiesText.text = "0";
        _playerExperienceSlider.maxValue = 100; //TODO: N1
        
        _enemySpawnController.KilledEnemyCountChangedEvent += UpdateKilledEnemiesText;
        _player.PlayerExperienceChangeEvent += UpdatePlayerExperienceText;
        _player.PlayerHealthChangeEvent += UpdatePlayerHealthText;
    }
    
    private void UpdateKilledEnemiesText(int killedEnemies)
    {
        _killedEnemiesText.text = killedEnemies.ToString();
    }

    private void UpdatePlayerExperienceText(int experience)
    {
        _playerExperienceSlider.value = experience;
    }

    private void UpdatePlayerHealthText(int health, bool isMax)
    {
        if (isMax)
        {
            _playerHealthSlider.maxValue = health;
        }
        _playerHealthSlider.value = health;
    }

    private void OnDestroy()
    {
        _enemySpawnController.KilledEnemyCountChangedEvent -= UpdateKilledEnemiesText;
        _player.PlayerExperienceChangeEvent -= UpdatePlayerExperienceText;
        _player.PlayerHealthChangeEvent -= UpdatePlayerHealthText;
    }
}

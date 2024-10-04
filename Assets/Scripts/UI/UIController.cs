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

    [Inject]
    private PlayerController _player;
    
    private void Start()
    {
        _killedEnemiesText.text = "0";
        _playerExperienceSlider.maxValue = 100; //TODO: N1
        
        _enemySpawnController.KilledEnemyCountChangedEvent += UpdateKilledEnemiesText;
        _player.PlayerExperienceChangeEvent += UpdatePlayerExperienceText;
    }
    
    private void UpdateKilledEnemiesText(int killedEnemies)
    {
        _killedEnemiesText.text = killedEnemies.ToString();
    }

    private void UpdatePlayerExperienceText(int experience)
    {
        _playerExperienceSlider.value = experience;
    }

    private void OnDestroy()
    {
        _enemySpawnController.KilledEnemyCountChangedEvent -= UpdateKilledEnemiesText;
        _player.PlayerExperienceChangeEvent -= UpdatePlayerExperienceText;
    }
}

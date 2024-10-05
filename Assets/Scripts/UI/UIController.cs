using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIController : MonoBehaviour
{
    public event Action<bool> ContinueGameEvent;
    
    [SerializeField] private Text _killedEnemiesText;
    [SerializeField] private Text _levelText;
    [SerializeField] private Slider _playerExperienceSlider;
    [SerializeField] private Slider _playerHealthSlider;
    [SerializeField] private GameObject _winPopup;
    [SerializeField] private GameObject _losePopup;

    [Inject] private PlayerController _player;
    [Inject] private GameManager _gameManager;

    private void Start()
    {
        _killedEnemiesText.text = "0";
        _levelText.text = "Lv. " + _gameManager.GetCurrentLevelNumber;

        _player.KilledEnemyCountChangedEvent += UpdateKilledEnemiesText;
        _player.PlayerExperienceChangeEvent += UpdatePlayerExperienceText;
        _player.PlayerHealthChangeEvent += UpdatePlayerHealthText;
        _player.LevelCompletedEvent += OnLevelComplete;
        _player.LevelFailedEvent += OnLevelFailed;

        _winPopup.SetActive(false);
        _losePopup.SetActive(false);
    }
    
    public void Init(LevelConfiguration levelConfiguration)
    {
        _playerExperienceSlider.maxValue = levelConfiguration.levelMaxExperience;
        _playerExperienceSlider.value = 0;

        _playerHealthSlider.maxValue = levelConfiguration.playerMaxHealth;
        _playerHealthSlider.value = levelConfiguration.playerMaxHealth;
        _killedEnemiesText.text = "0";
    }
    
    private void UpdateKilledEnemiesText(int killedEnemies)
    {
        _killedEnemiesText.text = killedEnemies.ToString();
    }

    private void UpdatePlayerExperienceText(int experience)
    {
        _playerExperienceSlider.value = experience;
    }

    private void UpdatePlayerHealthText(int health)
    {
        _playerHealthSlider.value = health;
    }

    private void OnLevelComplete()
    {
        ShowWinPopup();
        DOVirtual.DelayedCall(1.5f, () =>
        {
            ContinueGame(true);
        });
    }

    private void OnLevelFailed()
    {
        ShowLosePopup();
        DOVirtual.DelayedCall(1.5f, () =>
        {
            ContinueGame(false);
        });
    }

    private void ShowWinPopup()
    {
        _winPopup.SetActive(true);
    }
    
    private void ShowLosePopup()
    {
        _losePopup.SetActive(true);
    }

    private void ContinueGame(bool isWin)
    {
        DisableAllPopups();
        ContinueGameEvent?.Invoke(isWin);
    }

    private void DisableAllPopups()
    {
        _winPopup.SetActive(false);
        _losePopup.SetActive(false);
    }
    
    public void UpdateLevelText()
    {
        _levelText.text = "Lv. " + _gameManager.GetCurrentLevelNumber;
    }

    private void OnDestroy()
    {
        _player.KilledEnemyCountChangedEvent -= UpdateKilledEnemiesText;
        _player.PlayerExperienceChangeEvent -= UpdatePlayerExperienceText;
        _player.LevelCompletedEvent -= OnLevelComplete;
        _player.LevelFailedEvent -= OnLevelFailed;
    }
}

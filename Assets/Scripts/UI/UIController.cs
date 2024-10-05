using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIController : MonoBehaviour
{
    public event Action ReplayBtnClickedEvent;
    public event Action NextLevelBtnClickedEvent;
    
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
        _playerExperienceSlider.maxValue = _player.PlayerMaxExperience;
        _playerHealthSlider.maxValue = _player.GetMaxHealth;
        _playerHealthSlider.value = _player.GetMaxHealth;
        
        _player.KilledEnemyCountChangedEvent += UpdateKilledEnemiesText;
        _player.PlayerExperienceChangeEvent += UpdatePlayerExperienceText;
        _player.PlayerHealthChangeEvent += UpdatePlayerHealthText;
        _player.LevelCompletedEvent += OnLevelComplete;
        _player.LevelFailedEvent += OnLevelFailed;

        _winPopup.SetActive(false);
        _losePopup.SetActive(false);
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
    }

    private void OnLevelFailed()
    {
        ShowLosePopup();
    }

    private void ShowWinPopup()
    {
        _winPopup.SetActive(true);
    }
    
    private void ShowLosePopup()
    {
        _losePopup.SetActive(true);
    }

    public void OnNextLevelBtnClick()
    {
        DisableAllPopups();
        ResetUI();
        NextLevelBtnClickedEvent?.Invoke();
    }

    public void OnReplayBtnClick()
    {
        DisableAllPopups();
        ResetUI();
        ReplayBtnClickedEvent?.Invoke();
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

    private void ResetUI()
    {
        _killedEnemiesText.text = "0";
        _playerExperienceSlider.value = 0;
        _playerHealthSlider.value = _player.GetMaxHealth;
    }

    private void OnDestroy()
    {
        _player.KilledEnemyCountChangedEvent -= UpdateKilledEnemiesText;
        _player.PlayerExperienceChangeEvent -= UpdatePlayerExperienceText;
        _player.LevelCompletedEvent -= OnLevelComplete;
        _player.LevelFailedEvent -= OnLevelFailed;
    }
}

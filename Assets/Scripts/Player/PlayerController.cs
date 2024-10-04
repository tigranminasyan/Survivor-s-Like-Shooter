using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action<int> PlayerExperienceChangeEvent;

    [SerializeField] private PlayerHealthController _playerHealthController;
    [SerializeField, ReadonlyField] private int _playerExperience;
    
    public void GetDamage(int damage)
    {
        _playerHealthController.GetDamage(damage);
    }
    
    public void AddExperience(int experience)
    {
        _playerExperience += experience;
        PlayerExperienceChangeEvent?.Invoke(_playerExperience);
    }
}

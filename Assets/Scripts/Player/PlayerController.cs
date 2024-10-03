using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerHealthController _playerHealthController;
    
    public void GetDamage(int damage)
    {
        _playerHealthController.GetDamage(damage);
    }
}

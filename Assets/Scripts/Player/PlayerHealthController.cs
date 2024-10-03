using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField, Range(1, 25)] private int _health = 1;
    
    public void GetDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            //Debug.Log("Player is dead");//DEBUG: N1
        }
    }
}

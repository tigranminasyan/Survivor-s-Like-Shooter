using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private EnemySpawnController _enemySpawnController;
    [SerializeField] private Text _killedEnemiesText;
    
    private void Start()
    {
        _killedEnemiesText.text = "0";
        _enemySpawnController.KilledEnemyCountChangedEvent += UpdateKilledEnemiesText;
    }
    
    public void UpdateKilledEnemiesText(int killedEnemies)
    {
        _killedEnemiesText.text = killedEnemies.ToString();
    }
}

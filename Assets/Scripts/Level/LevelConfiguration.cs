using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Game Configuration/Level Configuration", order = 0)]

public class LevelConfiguration : ScriptableObject
{
    public int level;

    [Header("Level Settings")]
    public int levelMaxExperience;

    [Header("Player Settings")]

    public int playerMaxHealth;
    public float playerSpeed;
    public float enemyDetectRadius;
    
    [Header("Enemies Spawning Settings")]

    public List<EnemyConfiguration> enemyConfigurationContainerer;
    public float enemySpawnPeriod;
}

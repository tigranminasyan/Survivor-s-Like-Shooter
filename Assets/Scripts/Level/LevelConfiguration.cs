using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Game Configuration/Level Configuration", order = 1)]
public class LevelConfiguration : ScriptableObject
{
    public int level;

    [Header("Level Settings")]
    public int levelMaxExperience;

    [Header("Player Settings")]

    public int playerMaxHealth;
    public float playerSpeed;
    public float enemyDetectRadius;
}

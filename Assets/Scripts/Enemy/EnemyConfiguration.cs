using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Game Configuration/Enemy Configuration", order = 1)]

public class EnemyConfiguration : ScriptableObject
{
    public int health;
    public int damage;
    public float speed;
    public int grantingExperience;
    public EnemyController prefab;
}
using UnityEngine;

[CreateAssetMenu(fileName = "new Enemy", menuName = "Enemy/Create New Enemy")]
public class Enemy : ScriptableObject
{
    public GameObject Prefab;
    public int rateSpawn;
    public float EnemyDamage;
    public float MaxHealth;
    public Item ItemDrop;
}

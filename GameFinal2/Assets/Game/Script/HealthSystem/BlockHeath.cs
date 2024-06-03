using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHeath : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem DestroySystem;

    public float Health;

    public delegate void DestroyEnven();
    public DestroyEnven onDsetroy;

    public delegate void TakeDamageEvent(float damage, float Health);
    public TakeDamageEvent onTakeDamage;

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
            onTakeDamage?.Invoke(damage, Health);
            if (DestroySystem != null)
            {
                DestroySystem.gameObject.SetActive(true);
                DestroySystem.transform.SetParent(null, true);
                DestroySystem.Play();
            }
            onDsetroy?.Invoke();

            gameObject.SetActive(false);

            //SpawnRaid.instance.BakeNavMesh();

            Destroy(gameObject);
        }
        else
        {
            onTakeDamage?.Invoke(damage, Health);
        }
    }
}

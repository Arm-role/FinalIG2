using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem DestroySystem;

    public float health;

    public delegate void DestroyEnven();
    public DestroyEnven onDsetroy;

    public delegate void TakeDamageEvent(float damage, float Health);
    public TakeDamageEvent onTakeDamage;

    private Animator animator;

    private void Start()
    {
        if (TryGetComponent<Animator>(out Animator ani))
        {
            animator = ani;
        }
    }
    public void TakeDamage(float damage)
    {
        if(animator != null)
        {
            animator.SetTrigger("GetDamage");
        }
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            onTakeDamage?.Invoke(damage, health);
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
            onTakeDamage?.Invoke(damage, health);
        }
    }
}

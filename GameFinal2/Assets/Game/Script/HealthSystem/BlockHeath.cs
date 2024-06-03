using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHeath : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem DieParticle;
    [SerializeField]
    private ParticleSystem hitParticle;

    public float Health;

    public delegate void DestroyEnven();
    public DestroyEnven onDsetroy;

    public delegate void TakeDamageEvent(float damage, float Health);
    public TakeDamageEvent onTakeDamage;

    public void TakeDamage(float damage)
    {
        Health -= damage;

        if (hitParticle != null)
        {
            GameObject hit = Instantiate(hitParticle.gameObject, transform.position, transform.rotation);
            Destroy(hit, 2f);
        }
            
        if (Health <= 0)
        {
            Health = 0;
            onTakeDamage?.Invoke(damage, Health);
            if (DieParticle != null)
            {
                GameObject particle = Instantiate(DieParticle.gameObject, transform.position, transform.rotation);
                Destroy(particle, 2f);
                //DestroySystem.Play();
            }
            onDsetroy?.Invoke();

            gameObject.SetActive(false);

            Destroy(gameObject);
        }
        else
        {
            onTakeDamage?.Invoke(damage, Health);
        }
    }
}

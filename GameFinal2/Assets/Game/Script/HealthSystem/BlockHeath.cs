using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHeath : MonoBehaviour
{
    public float Health;

    public delegate void DestroyEnven();
    public DestroyEnven onDsetroy;

    public delegate void TakeDamageEvent(float damage, float Health);
    public TakeDamageEvent onTakeDamage;
    public void TakeDamage(float damage)
    {
        Health -= damage;

        if (ParticleManager.instance.HitBlock != null)
        {
            GameObject hit = Instantiate(ParticleManager.instance.HitBlock.gameObject, transform.position, transform.rotation);
            hit.transform.localScale = transform.localScale / 2;
            Destroy(hit, 2f);
        }
            
        if (Health <= 0)
        {
            Health = 0;
            onTakeDamage?.Invoke(damage, Health);
            if (ParticleManager.instance.BockDestroy != null)
            {
                GameObject particle = Instantiate(ParticleManager.instance.BockDestroy.gameObject, transform.position, transform.rotation);
                Destroy(particle, 2f);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
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

        if (ParticleManager.instance.Hit != null)
        {
            GameObject hit = Instantiate(ParticleManager.instance.Hit.gameObject, transform.position, transform.rotation);
            hit.transform.localScale = transform.localScale / 2;
            Destroy(hit, 2f);
        }

        if (health <= 0)
        {
            health = 0;
            onTakeDamage?.Invoke(damage, health);
            if (ParticleManager.instance.Death != null)
            {
                GameObject particle = Instantiate(ParticleManager.instance.Death.gameObject, transform.position, transform.rotation);
                Destroy(particle, 2f);
            }
            onDsetroy?.Invoke();

            gameObject.SetActive(false);

            Destroy(gameObject);
        }
        else
        {
            onTakeDamage?.Invoke(damage, health);
        }
    }
}

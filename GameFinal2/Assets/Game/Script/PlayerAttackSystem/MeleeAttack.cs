using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public Vector3 Offset;
    public float Radien;
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 origin = transform.position - Offset;
        Collider[] colliders = Physics.OverlapSphere(origin, Radien);
        foreach (Collider collider in colliders)
        {
            if (!collider.isTrigger)
            {
                if (collider.CompareTag("Enemy"))
                {
                    if (collider.transform.TryGetComponent<Health>(out Health health))
                    {
                        Debug.Log("TakeDamage");
                        PlayerAttack.Instance.Attack(health);
                        break;
                    }
                    //Quaternion HitRotate = transform.rotation * Quaternion.Euler(0, 180, 0);

                    //GameObject hitOB = Instantiate(ParticleManager.instance.Hit.gameObject,
                    //hit.point, HitRotate);

                    //Destroy(hitOB, 2f);
                }
                else
                {
                    //Quaternion FlashRotate = transform.rotation * Quaternion.Euler(0, 90, 0);

                    //GameObject hitOB = Instantiate(ParticleManager.instance.Flash.gameObject,
                    //hit.point, FlashRotate);

                    //Destroy(hitOB, 2f);
                }
            }
        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position - Offset, Radien);
    }
}

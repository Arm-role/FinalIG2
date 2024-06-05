using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class UsePlant : MonoBehaviour
{
    public static UsePlant Instance;

    public Item item;
    public LayerMask layerMask;
    public float rayLength = 10f;
    public bool isPress = false;
    public bool isPress2 = false;

    public Transform HitOrigin;
    public float GunLength = 50f;

    public RaycastHit[] Allhit;

    public bool HaveSeed = false;
    public bool foundSoil = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }
    private void Update()
    {
        RayCastSystem();
    }
    public void _placeSeed(Item item)
    {
        if (foundSoil)
        {
            this.item = item;
            isPress2 = true;
        }
    }
    private void RayCastSystem()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        RaycastHit[] hits = Physics.RaycastAll(origin, direction, rayLength, layerMask);
        List<RaycastHit> col = new List<RaycastHit>();
        Allhit = hits;
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.name == "model" || hit.collider.gameObject.name == "Plant")
            {
                col.Add(hit);
                GrowSystem grow = hit.collider.transform.parent.GetComponent<GrowSystem>();
                HaveSeed = grow.HavePlant;
                foundSoil = true;

                if (isPress2)
                {
                    grow.GetSeed(item);
                    isPress2 = false;
                }
            }
        }
        if (col.Count == 0)
        {
            //ถ้าไม่เจอsoil
            foundSoil = false;
            isPress2 = false;
        }
    }
    public void HavestPlant()
    {
        if (foundSoil)
        {
            foreach (RaycastHit hit in Allhit)
            {
                if (hit.collider.gameObject.name == "model" || hit.collider.gameObject.name == "Plant")
                {
                    GrowSystem grow = hit.collider.transform.parent.GetComponent<GrowSystem>();
                    grow.Havest();
                    break;
                }
            }
        }
    }
    public void ShootGun()
    {
        if (ParticleManager.instance != null)
        {
            Vector3 origin = transform.position;
            Vector3 direction = transform.forward;

            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, GunLength))
            {
                if (!hit.collider.isTrigger)
                {
                    GameObject particle = Instantiate(ParticleManager.instance.MuzzleFlash.gameObject,
                    HitOrigin.position, HitOrigin.rotation);

                    Destroy(particle, 2f);

                    if (hit.collider.CompareTag("Enemy"))
                    {
                        if (hit.collider.transform.TryGetComponent<Health>(out Health health))
                        {
                            Debug.Log("TakeDamage");
                            PlayerAttack.Instance.Attack(health);
                        }
                        Quaternion HitRotate = transform.rotation * Quaternion.Euler(0, 180, 0);

                        GameObject hitOB = Instantiate(ParticleManager.instance.Hit.gameObject,
                        hit.point, HitRotate);

                        Destroy(hitOB, 2f);
                    }
                    else
                    {
                        Quaternion FlashRotate = transform.rotation * Quaternion.Euler(0, 90, 0);

                        GameObject hitOB = Instantiate(ParticleManager.instance.Flash.gameObject,
                        hit.point, FlashRotate);

                        Destroy(hitOB, 2f);
                    }
                }
            }
        }
    }
    public void MeleeAttack()
    {
        if (ParticleManager.instance != null)
        {
            Vector3 origin = transform.position;
            Vector3 direction = transform.forward;

            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, GunLength))
            {
                if (!hit.collider.isTrigger)
                {
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        if (hit.collider.transform.TryGetComponent<Health>(out Health health))
                        {
                            Debug.Log("TakeDamage");
                            PlayerAttack.Instance.Attack(health);
                        }
                        Quaternion HitRotate = transform.rotation * Quaternion.Euler(0, 180, 0);

                        GameObject hitOB = Instantiate(ParticleManager.instance.Hit.gameObject,
                        hit.point, HitRotate);

                        Destroy(hitOB, 2f);
                    }
                    else
                    {
                        Quaternion FlashRotate = transform.rotation * Quaternion.Euler(0, 90, 0);

                        GameObject hitOB = Instantiate(ParticleManager.instance.Flash.gameObject,
                        hit.point, FlashRotate);

                        Destroy(hitOB, 2f);
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * rayLength);
    }
}

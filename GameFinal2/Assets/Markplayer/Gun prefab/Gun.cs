using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float fireRate = 0.4f;
    private float nextFire = 0.0f;
    public GameObject bullet;
    public GameObject muzzle;

    void Start()
    {
        muzzle.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Fire();
            muzzle.SetActive(true);
            StartCoroutine(MuzzleOff(0.15f));
        }
    }

    public void Fire()
    {
        Instantiate(bullet, transform.position, transform.rotation);
    }

    IEnumerator MuzzleOff(float secondUntildestroy)
    {
        yield return new WaitForSeconds(secondUntildestroy);
        muzzle.SetActive(false);
    }
}
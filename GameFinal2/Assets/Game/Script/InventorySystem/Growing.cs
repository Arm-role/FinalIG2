using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Growing : MonoBehaviour
{
    private float delay = 5f; // เวลาที่จะทำลาย example
    private float respawnTime = 6.0f; // เวลาที่จะสร้าง test ขึ้นมาใหม่
    private float respawnTime2 = 11.0f;
    public GameObject plant1;
    public GameObject plant2;
    public GameObject plant3;
    private GameObject pot;


    void Start()
    {        
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;
        StartCoroutine(DestroyAndRespawn(position, rotation));        
    }

    private IEnumerator DestroyAndRespawn(Vector3 position, Quaternion rotation)
    {
        GameObject p1 = Instantiate(plant1, transform);
        // รอเวลาที่กำหนดก่อนทำลาย example
        yield return new WaitForSeconds(delay);
        Destroy(p1);

        GameObject p2 = Instantiate(plant2, transform);
        // รอเวลาที่กำหนดก่อนสร้าง GameObject ใหม่
        yield return new WaitForSeconds(respawnTime);
        Destroy(p2);

        GameObject p3 = Instantiate(plant3, transform);
        yield return new WaitForSeconds(respawnTime2);

    }
}   

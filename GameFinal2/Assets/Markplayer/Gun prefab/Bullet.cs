using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bullet : MonoBehaviour
{
    public float Speed = 0.7f;
    public float SecondsUntilDestroy = 1.2f;
    float startTime;
    void Start()
    {
        startTime = Time.time;
    }
    void Update()
    {
        this.gameObject.transform.position += Speed * this.gameObject.transform.forward;
        if (Time.time - startTime >= SecondsUntilDestroy)
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
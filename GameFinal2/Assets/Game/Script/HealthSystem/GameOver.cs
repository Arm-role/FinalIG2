using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject player;
    private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    private void Update()
    {
        if(virtualCamera.Follow == null)
        {
            GameObject surrugate = new GameObject();
            surrugate.transform.position = transform.position;
            virtualCamera.Follow = surrugate.transform;
            Debug.Log("GameOver");
        }
    }
}

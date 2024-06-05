using Cinemachine;
using UnityEngine;

public class GameOver : MonoBehaviour
{
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

            UIActive.instance.GameOver.gameObject.SetActive(true);
            Debug.Log("GameOver");
        }
    }
}

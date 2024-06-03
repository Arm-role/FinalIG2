using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject player;

    private void Update()
    {
        if (player == null)
        {
            Debug.Log("GameOver");
        }
    }
}

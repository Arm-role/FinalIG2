using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    // Update is called once per frame

    public void Startgame()
    {
        SceneManager.LoadScene(1);
    }
}


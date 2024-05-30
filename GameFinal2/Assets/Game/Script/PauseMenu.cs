using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;

    public void Continue()
    {
        PausePanel.SetActive(false);
    }
}

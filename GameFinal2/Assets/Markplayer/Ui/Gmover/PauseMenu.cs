using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;

    public void Continue()
    {
        PausePanel.SetActive(false);
    }
    public void Exit(int sceneLevel)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneLevel);
    }
}

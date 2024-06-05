using UnityEngine;
using UnityEngine.SceneManagement;

public class gameOverUI : MonoBehaviour
{
    public void Continue()
    {
        SceneManager.LoadScene(1);
        Debug.Log("restart");
    }
    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        Debug.Log("Menu");
    }
    public void Exit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}

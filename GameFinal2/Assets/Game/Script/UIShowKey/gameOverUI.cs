using UnityEngine;
using UnityEngine.SceneManagement;

public class gameOverUI : MonoBehaviour
{
    public void Continue()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void Exit()
    {
        Application.Quit();
    }
}

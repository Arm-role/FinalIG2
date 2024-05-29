using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowBuildMode : MonoBehaviour
{
    public static ShowBuildMode instance;
    public TextMeshProUGUI Text;
    public Image Image;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

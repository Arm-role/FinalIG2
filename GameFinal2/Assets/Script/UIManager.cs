using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Color ColorBackground;
    public Color HotColorBackground;

    public Image[] images;
    public Image[] Hotbar;

    public void ChangeColorImage()
    {
        foreach (Image image in images)
        {
            if (image != null)
            {
                image.color = ColorBackground;
            }
        }
        foreach (Image image in Hotbar)
        {
            if (image != null)
            {
                image.color = HotColorBackground;
            }
        }
    }
}

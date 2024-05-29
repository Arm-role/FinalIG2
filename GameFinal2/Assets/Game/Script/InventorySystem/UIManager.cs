using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Color ColorBackground;
    public Color SlotColor;

    [Space(10)]
    public Transform[] UIAll;

    public void ChangeColorImage()
    {
        for (int i = 0; i < UIAll.Length; i++)
        {
            if (UIAll[i].childCount >= 1)
            {
                for (int x = 0; x < UIAll[i].childCount; x++)
                {
                    Image Slot = UIAll[i].GetChild(x).GetComponent<Image>();
                    Debug.Log(UIAll[i].name + " : " + UIAll[i].GetChild(x).name);
                    Slot.color = SlotColor;
                }
            }
            
            Image background = UIAll[i].GetComponent<Image>();
            background.color = ColorBackground;
        }
    }
}

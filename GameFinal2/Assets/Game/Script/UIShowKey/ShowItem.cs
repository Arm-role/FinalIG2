using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowItem : MonoBehaviour
{
    public static ShowItem Instace;
    public GameObject ItemOnUI;
    public Color AddColor;
    public Color RemoveColor;

    private void Awake()
    {
        if (Instace == null)
        {
            Instace = this;
        }else
        {
            Destroy(gameObject);

        }
    }
    public void ShowOb(Item item, bool isAdd)
    {
        GameObject OB = Instantiate(ItemOnUI, transform);
        var Name = OB.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        var colorBG = OB.GetComponent<Image>();
        var sprite = OB.transform.Find("icon").GetComponent<Image>();

        if (isAdd)
        {
            colorBG.color = AddColor;
        }
        else
        {
            colorBG.color = RemoveColor;
        }
        Name.text = item.itemName;
        sprite.sprite = item.sprite;
    }
}

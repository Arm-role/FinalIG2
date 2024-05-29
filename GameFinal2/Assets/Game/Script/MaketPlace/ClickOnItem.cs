using UnityEngine;
using UnityEngine.EventSystems;

public class ClickOnItem : MonoBehaviour, IPointerClickHandler
{
    public Item item;
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        MaketplaceManager.Instance.ShowItemInfo(item);
    }
}

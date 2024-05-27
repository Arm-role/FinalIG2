using StarterAssets;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{

    public Item Item;
    public bool _isHotbar;
    public int Value;
    public void UseItem(int value)
    {
        if(Item.itemType == ItemType.Building)
        {
            if (BuildingManager.Instance.canBuilding)
            {
                InventoryManager.Instance.Remove(Item, value);
            }
        }
    }
}

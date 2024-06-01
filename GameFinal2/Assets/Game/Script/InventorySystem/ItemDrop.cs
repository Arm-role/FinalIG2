using StarterAssets;
using UnityEngine;
using UnityEngine.Windows;

public class ItemDrop : MonoBehaviour
{

    public Item Item;
    public bool _isHotbar;
    public int Value;
    public void UseItem(int value)
    {
        switch (Item.itemType)
        {
            case ItemType.Misc:
                //Debug.Log(UsePlant.Instance.HaveSeed + " : " + UsePlant.Instance.foundSoil);
                if (!UsePlant.Instance.HaveSeed && UsePlant.Instance.foundSoil)
                {
                    InventoryManager.Instance.Remove(Item, value);
                }
                break;

            case ItemType.Weapon:
                break;

            case ItemType.Building:
                if (BuildingManager.Instance.canBuilding)
                {
                    InventoryManager.Instance.Remove(Item, value);
                }
                break;

            case ItemType.Gun:
                break;
        }
    }
}

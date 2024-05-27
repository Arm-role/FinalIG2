using System.Collections.Generic;
using UnityEngine;

public class SellingSystem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SellArea"))
        {
            if(InventoryManager.Instance.items != null)
            {
                List<(Item, int)> ItemToRemove = new List<(Item, int)>();

                foreach (var item in InventoryManager.Instance.items)
                {
                    if (item.Item1.itemType == ItemType.Misc)
                    {
                        Debug.Log(item.Item1.itemName);
                        MyMoneySystem.instance.Money += (item.Item2 * item.Item1.price);
                        MyMoneySystem.instance.ShowOnInventory();
                        ItemToRemove.Add((item.Item1, item.Item2));
                    }
                }
                foreach (var item in ItemToRemove)
                {
                    InventoryManager.Instance.Remove(item.Item1, item.Item2);
                }
            }
        }
    }
}

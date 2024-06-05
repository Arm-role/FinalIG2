using System.Collections.Generic;
using UnityEngine;

public class Advice : MonoBehaviour
{
    public static Advice Instance;

    public int showAdvice;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        switch (showAdvice)
        {
            case 1:
                AdviceShowUI.instance.IsActiveOnScene(true);
                break;

            case 2:
                AdviceShowUI.instance.IsActiveOnScene(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (InventoryManager.Instance.items != null)
                    {
                        List<(Item, int)> ItemToRemove = new List<(Item, int)>();

                        foreach (var item in InventoryManager.Instance.items)
                        {
                            if (item.Item1.itemType == ItemType.Misc)
                            {
                                //Debug.Log(item.Item1.itemName);
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
                break;
            case 0:
                AdviceShowUI.instance.IsActiveOnScene(false);
            break;
        }
    }
}

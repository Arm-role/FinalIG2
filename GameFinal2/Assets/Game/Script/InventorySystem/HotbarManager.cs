using StarterAssets;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotbarManager : MonoBehaviour
{
    public Transform hotbar;
    private List<Transform> SlotList = new List<Transform>();

    bool isCreate = false;

    private StarterAssetsInputs _input;
    private void Start()
    {
        _input = FindAnyObjectByType<StarterAssetsInputs>();

        foreach (Transform slot in hotbar)
        {
            SlotList.Add(slot);
        }
    }
    private void Update()
    {
        if (InventoryManager.Instance.ValueChang)
        {
            DestroyItem();
            CreatItem();
            InventoryManager.Instance.ValueChang = false;
        }
        if (hotbar != null)
        {
            if (_input.OpenInventory)
            {
                DestroyItem();
            }
            else
            {
                if (isCreate)
                {
                    CreatItem();
                }
            }
        }
    }
    private void CreatItem()
    {
        foreach (Transform slot in SlotList)
        {
            foreach (var item in InventoryManager.Instance.items)
            {
                if (item.Item4)
                {
                    if (item.Item3.parent.name == slot.parent.name)
                    {
                        if (item.Item3.name == slot.name)
                        {
                            InventoryManager.Instance.CreatItem(item.Item1, item.Item2, slot, item.Item4);
                            isCreate = false;
                        }
                    }
                }
            }
        }
    }
    private void DestroyItem()
    {
        isCreate = true;
        foreach (Transform slot in SlotList)
        {
            if (slot.childCount > 0)
            {
                Destroy(slot.GetChild(0).gameObject);
            }
        }
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<(Item, int, Transform, bool)> items = new List<(Item, int, Transform, bool)>();

    public GameObject InventoryItem;
    public TextMeshProUGUI TextCoin;

    [SerializeField] private Transform[] ParrentSlot;
    public List<Transform> Slots = new List<Transform>();
    public bool ValueChang = false;
    public int GetSlot = 0;

    bool SlotOnHot = false;

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
    void Start()
    {
        foreach (Transform parentSlot in ParrentSlot)
        {
            for (int i = 0; i < parentSlot.childCount; ++i)
            {
                Slots.Add(parentSlot.GetChild(i));
            }
        }
    }

    public void Add(Item item)
    {
        ValueChang = true;
        if (item.IsStackable)
        {
            bool ItemAlradyInInventory = false;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Item1.id == item.id)
                {
                    items[i] = (items[i].Item1, items[i].Item2 + item.value, items[i].Item3, items[i].Item4);
                    ShowItem.Instace.ShowOb(item, true);
                    ItemAlradyInInventory = true;
                }
            }
            if (!ItemAlradyInInventory)
            {
                items.Add((item, 1, SlotSystem(Slots), SlotOnHot));
                ShowItem.Instace.ShowOb(item, true);
            }
        }
        else
        {
            items.Add((item, 1, SlotSystem(Slots), SlotOnHot));
            ShowItem.Instace.ShowOb(item, true);
        }
    }
    public void Remove(Item item, int value)
    {
        ValueChang = true;
        if (item.IsStackable)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Item1.id == item.id)
                {
                    if ((items[i].Item2 - value) > 1)
                    {
                        items[i] = (items[i].Item1, items[i].Item2 - value, items[i].Item3, items[i].Item4);
                        ShowItem.Instace.ShowOb(item, false);
                        Debug.Log("press");
                    }
                    else
                    {
                        items.Remove(items[i]);
                        ShowItem.Instace.ShowOb(item, false);
                        Debug.Log("Remove");
                    }
                }
            }
        }
    }
    public void ListItem()
    {
        if (Instance != null)
        {
            MyMoneySystem.instance.ShowOnInventory(TextCoin);
            DestroyItem();
            foreach (var item in items)// item มีอยู่ใน List หรือป่าว
            {
                CreatItem(item.Item1, item.Item2, item.Item3, item.Item4);
            }
        }
    }
    private Transform SlotSystem(List<Transform> Slots)
    {
        bool CheckSlot = false;
        List<string> instanceSlot = new List<string>();

        if (items.Count == 0)
        {
            SlotOnHotbar(Slots[0]);
            return Slots[0];
        }
        else if (items.Count > 0)
        {
            Debug.Log(items.Count);
            for (int i = 0; i < Slots.Count; i++) 
            {
                foreach (var item in items)
                {
                    if (Slots[i].name == item.Item3.name)
                    {
                        instanceSlot.Add(item.Item3.name);
                        Debug.Log("Add : " + item.Item3.name);
                    }
                }
            }
            CheckSlot = true;
        }
        if (CheckSlot)
        {
            int i = 0;
            foreach (var slot in Slots)
            {
                if (instanceSlot != null)
                {
                    if (i < instanceSlot.Count)
                    {
                        if (slot.name != instanceSlot[i])
                        {
                            SlotOnHotbar(slot);
                            return slot;
                        }
                        else if (slot.name == instanceSlot[i])
                        {
                            i++;
                        }
                    }
                    else
                    {
                        SlotOnHotbar(slot);
                        return slot;
                    }
                }
            }
        }
        return null;
    }
    public void SlotOnHotbar(Transform slot)
    {
        if (slot.parent.name == "Hotbar")
        {
            SlotOnHot = true;
        }else
        {
            SlotOnHot = false;
        }
        Debug.Log("ChackHotbar");
    }
    public void SetItem(int i, Transform transform, bool OnHotbar)
    {
        items[i] = (items[i].Item1, items[i].Item2, transform, OnHotbar);
    }
    public void DebugItems()
    {
        foreach (var item in items)
        {
            Debug.Log(item.Item1.itemName + " : " + item.Item2 + " : " + item.Item3 + " : " + item.Item4);
        }
    }
    public void CreatItem(Item item, int Value ,Transform transform, bool OnHotbar)
    {
        GameObject ob = Instantiate(InventoryItem, transform);

        var itemName = ob.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        var itemValue = ob.transform.Find("Value").GetComponent<TextMeshProUGUI>();
        var itemIcon = ob.transform.Find("Icon").GetComponent<Image>();
        var itemData = ob.transform.GetComponent<ItemDrop>();

        itemName.text = item.itemName;
        itemValue.text = Value.ToString();
        itemIcon.sprite = item.sprite;

        itemData.Item = item;
        itemData._isHotbar = OnHotbar;
        itemData.Value = Value;
    }
    public void DestroyItem()
    {
        foreach (Transform slot in Slots)
        {
            foreach (Transform item in slot)
            {
                Destroy(item.gameObject);
            }
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaketplaceManager : MonoBehaviour
{
    public static MaketplaceManager Instance;

    public Transform BuyFrame;

    public TextMeshProUGUI YourCoin;
    public TextMeshProUGUI priceValue;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;

    public Image ItemImage;

    public Item[] ItemOnSell;

    public GameObject InventoryItem;
    public GameObject Slot;

    private Item currentItem;

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
    public void ShowItemInfo(Item item)
    {
        Name.text = item.itemName;
        Description.text = item.Desciption;
        priceValue.text = item.price.ToString();
        ItemImage.sprite = item.sprite;
        currentItem = item;
    }
    public void ListItem()
    {
        MyMoneySystem.instance.ShowOnInventory(YourCoin);
        DestroyItem();
        foreach (var item in ItemOnSell)
        {
            CreatItem(item);
        }
    }
    public void CreatItem(Item item)
    {
        GameObject ob = Instantiate(Slot, BuyFrame);

        if (ob != null)
        {
            GameObject Item = Instantiate(InventoryItem, ob.transform);

            var itemIcon = Item.transform.Find("Icon").GetComponent<Image>();
            var itemData = Item.transform.GetComponent<ClickOnItem>();

            itemIcon.sprite = item.sprite;
            itemData.item = item;
        }
    }
    public void BuyItem()
    {
        if (MyMoneySystem.instance.Money > currentItem.price)
        {
            if ((MyMoneySystem.instance.Money - currentItem.price) >= 0)
            {
                MyMoneySystem.instance.Money -= currentItem.price;
                if (currentItem != null)
                {
                    InventoryManager.Instance.Add(currentItem);
                }
                MyMoneySystem.instance.ShowOnInventory(YourCoin);
            }
        }
    }
    public void DestroyItem()
    {
        foreach (Transform slot in BuyFrame)
        {
            Destroy(slot.gameObject);
        }
    }
}

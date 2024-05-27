using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "new Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    public int id;
    public ItemType itemType;
    public string itemName;
    public int value;
    public int price;
    public Sprite sprite;
    public bool IsStackable;
    public GameObject ObjectOnWorld;
    public string Desciption;
}

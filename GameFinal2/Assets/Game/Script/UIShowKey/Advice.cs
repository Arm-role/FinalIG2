using System.Collections.Generic;
using UnityEngine;

public class Advice : MonoBehaviour
{
    public static Advice Instance;
    public Vector3 Positon;
    public float sphereRadius;
    public float maxDistance = 10f;
    public int showAdvice;
    public LayerMask showAdviceMask;
    public bool Onseller = false;

    public Vector3 boxSize = new Vector3(2, 2, 2);

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
        Vector3 origin = transform.position + Positon;

        Collider[] Collider = Physics.OverlapBox(origin, boxSize / 2, Quaternion.identity, showAdviceMask);

        List<Collider> coll = new List<Collider>();
        foreach (Collider hit in Collider)
        {
            if (hit.GetComponent<Collider>().CompareTag("SellArea"))
            {
                coll.Add(hit);
            }
            else if (hit.GetComponent<Collider>().CompareTag("AdviceArea"))
            {
                coll.Add(hit);
            }
        }
        if (coll.Count > 0)
        {
            foreach (Collider hit in coll)
            {
                if (hit.GetComponent<Collider>().CompareTag("SellArea"))
                {
                    showAdvice = 2;
                    Onseller = true;
                }
                else if (hit.GetComponent<Collider>().CompareTag("AdviceArea"))
                {
                    showAdvice = 1;
                    Onseller = false;
                }
            }
        }else
        {
            showAdvice = 0;
        }
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
                break;
            case 0:
                AdviceShowUI.instance.IsActiveOnScene(false);
            break;
        }
    }
    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position + Positon;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(origin, boxSize);
    }
}

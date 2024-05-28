using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GrowSystem : MonoBehaviour
{
    private GameObject ObID;

    public float timer = 0;
    public float timeSpeed;

    private GameObject plantOB;

    private bool isCreate = true;
    private bool isGrow = true;

    private Item item;
    void Update()
    {
        if (isGrow)
        {
            if (ObID != null)
            {
                timer += Time.deltaTime * timeSpeed;

                switch (timer)
                {
                    case < 10:
                        if (isCreate)
                        {
                            Debug.Log("State1");
                            CreateOB(ObID.transform.GetChild(0).gameObject);
                            isCreate = false;
                        }
                        break;
                    case < 11:
                        isCreate = true;
                        break;
                    case < 20:
                        if (isCreate)
                        {
                            Debug.Log("State2");
                            Destroy(plantOB);
                            CreateOB(ObID.transform.GetChild(1).gameObject);
                            isCreate = false;
                        }
                        break;
                    case < 21:
                        isCreate = true;
                        break;
                    case > 22:
                        isCreate = true;
                        if (isCreate)
                        {
                            Debug.Log("State3");
                            Destroy(plantOB);
                            CreateOB(ObID.transform.GetChild(2).gameObject);
                            isCreate = false;
                            isGrow = false;
                        }
                        break;
                }
            }
        }
    }
    private void CreateOB(GameObject Ob)
    {
        plantOB = Instantiate(Ob, transform);
        plantOB.transform.position = transform.GetChild(0).position;
    }
    public void GetSeed(Item item)
    {
        this.item = item;
        ObID = item.ObjectOnWorld;
        Debug.Log("get");
    }
    public void Havest()
    {
        if (!isGrow)
        {
            if(item != null)
            {
                InventoryManager.Instance.Add(item);
                Destroy(plantOB);
                isGrow = true;
                isCreate = true;
                plantOB = null;
                ObID = null;
                timer = 0;
            }
        }
    }
}

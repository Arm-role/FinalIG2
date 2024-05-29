using System.Collections.Generic;
using UnityEngine;

public class UsePlant : MonoBehaviour
{
    public static UsePlant Instance;

    public Item item;
    public LayerMask layerMask;
    public float rayLength = 10f;
    public bool isPress = false;
    public bool isPress2 = false;

    public RaycastHit[] Allhit;

    public bool HaveSeed = false;
    public bool foundSoil = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }
    private void Update()
    {
        RayCastSystem();
    }
    public void _placeSeed(Item item)
    {
        if (foundSoil)
        {
            this.item = item;
            isPress2 = true;
        }
    }
    private void RayCastSystem()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        RaycastHit[] hits = Physics.RaycastAll(origin, direction, rayLength, layerMask);
        List<RaycastHit> col = new List<RaycastHit>();
        Allhit = hits;
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.name == "model" || hit.collider.gameObject.name == "Plant")
            {
                col.Add(hit);
                GrowSystem grow = hit.collider.transform.parent.GetComponent<GrowSystem>();
                HaveSeed = grow.HavePlant;
                foundSoil = true;

                if (isPress2)
                {
                    grow.GetSeed(item);
                    isPress2 = false;
                }
            }
        }
        if (col.Count == 0)
        {
            //ถ้าไม่เจอsoil
            foundSoil = false;
            isPress2 = false;
        }
    }
    public void HavestPlant()
    {
        if (foundSoil)
        {
            foreach (RaycastHit hit in Allhit)
            {
                if (hit.collider.gameObject.name == "model" || hit.collider.gameObject.name == "Plant")
                {
                    GrowSystem grow = hit.collider.transform.parent.GetComponent<GrowSystem>();
                    grow.Havest();
                    break;
                }
            }
        }
    }
}

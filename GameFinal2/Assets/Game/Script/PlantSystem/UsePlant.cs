using UnityEngine;

public class UsePlant : MonoBehaviour
{
    public static UsePlant Instance;

    public Item item;
    public LayerMask layerMask;
    public float rayLength = 10f;
    public bool isPress = false;
    public bool isPress2 = false;

    public RaycastHit hit;

    public bool HaveSeed = false;
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
        if(item != null)
        {
            if (item.itemName == "SoilPlant")
            {
                RayCastSystem();
            }
        }
    }
    public void _placeSeed(Item item)
    {
        this.item = item;
        isPress2 = true;
    }
    private void RayCastSystem()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        Debug.DrawRay(origin, direction * rayLength, Color.red);

        RaycastHit[] hits = Physics.RaycastAll(origin, direction, rayLength);

        foreach (RaycastHit hit in hits)
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.name == "model")
            {
                this.hit = hit;
                GrowSystem grow = hit.collider.transform.parent.GetComponent<GrowSystem>();
                HaveSeed = grow.HavePlant;

                if (isPress2)
                {
                    grow.GetSeed(item);
                    isPress2 = false;
                }
                else
                {
                    if (isPress)
                    {
                        grow.Havest();
                        isPress = false;
                        Debug.Log("havest");
                    }
                }
            }
        }
    }
}

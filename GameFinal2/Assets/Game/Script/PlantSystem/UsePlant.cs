using UnityEngine;

public class UsePlant : MonoBehaviour
{
    public static UsePlant Instance;

    public Item item;
    public LayerMask layerMask;
    public float rayLength = 10f;
    public bool isPress = false;

    public RaycastHit hit;
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
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        Debug.DrawRay(origin, direction * rayLength, Color.red);

        if (Physics.Raycast(origin, direction, out hit, rayLength))
        {
            if (hit.collider.gameObject.name == "SoilBox" || hit.collider.gameObject.name == "CollBox")
            {
                GrowSystem grow = hit.collider.transform.GetComponent<GrowSystem>();
                if (item != null)
                {
                        grow.GetSeed(item);
                        item = null;
                }else
                {
                    if(isPress)
                    {
                        grow.Havest();
                        isPress = false;
                        Debug.Log("havest");
                    }
                }
            }
        }
    }
    public void _placeSeed(Item item)
    {
        this.item = item;
    }
}

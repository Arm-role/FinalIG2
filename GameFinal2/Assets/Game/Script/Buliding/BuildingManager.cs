using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;
    public float scaleing;
    private GameObject objects;
    public Material canBuild;
    public Material cantBuild;
    public float rayLength = 10f;
    private GameObject panddingObject;

    private Vector3 pos;
    Vector3 roundedPos;
    private int ItemID;

    private RaycastHit hit;
    [SerializeField] private LayerMask layerMask;

    public float gridSize;
    public float RotateAmount;

    public bool isCreateOB = false;

    bool _isHolding = true;
    bool gridOn = true;
    bool OnBox = false;
    public bool canBuilding = true;
    public Item Item;

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
    private void Update()
    {
        if (panddingObject != null)
        {
            UpdatePandingObjectPosition();

            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateObject();
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            gridOn = !gridOn;
        }
    }

    private void FixedUpdate()
    {
        UpdateRaycastHit();
    }

    private void UpdatePandingObjectPosition()
    {
        bool isCollisionBox = false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 50, layerMask))
        {
            if (hit.collider.CompareTag("block"))
            {
                Vector3 distance = hit.collider.transform.position - hit.point;
                isCollisionBox = true;

                if (distance.x == -0.5f)
                {
                    //Debug.Log("Left");
                    roundedPos = new Vector3(
                    RoundToNearestGrid(pos.x) - distance.x,
                    RoundToNearestGrid(pos.y),
                    RoundToNearestGrid(pos.z) - 0.5f
                    );
                }

                else if (distance.x == 0.5f)
                {
                    //Debug.Log("Right");
                    roundedPos = new Vector3(
                    RoundToNearestGrid(pos.x) - distance.x,
                    RoundToNearestGrid(pos.y),
                    RoundToNearestGrid(pos.z) - 0.5f
                    );
                }

                else if (distance.z == -0.5f)
                {
                    //Debug.Log("Back");
                    roundedPos = new Vector3(
                    RoundToNearestGrid(pos.x) - 0.5f,
                    RoundToNearestGrid(pos.y),
                    RoundToNearestGrid(pos.z) - distance.z
                    );
                }

                else if (distance.z == 0.5f)
                {
                    //Debug.Log("front");
                    roundedPos = new Vector3(
                    RoundToNearestGrid(pos.x) - 0.5f,
                    RoundToNearestGrid(pos.y),
                    RoundToNearestGrid(pos.z) - distance.z
                    );
                }

                else if (distance.y == -0.5f)
                {
                    //Debug.Log("Up");
                    roundedPos = new Vector3(
                    RoundToNearestGrid(pos.x) - 0.5f,
                    RoundToNearestGrid(pos.y) - distance.y * 2,
                    RoundToNearestGrid(pos.z) - 0.5f
                    );
                    OnBox = true;
                }

                else if (distance.y == 0.5f)
                {
                    //Debug.Log("Down");
                    roundedPos = new Vector3(
                    RoundToNearestGrid(pos.x) - 0.5f,
                    RoundToNearestGrid(pos.y) - distance.y * 2,
                    RoundToNearestGrid(pos.z) - 0.5f
                    );
                    OnBox = true;
                }
            }
            else
            {
                isCollisionBox = false;
            }
        }
        if (gridOn)
        {
            if (!isCollisionBox)
            {
                roundedPos = new Vector3(
                RoundToNearestGrid(pos.x) - 0.5f,
                RoundToNearestGrid(pos.y),
                RoundToNearestGrid(pos.z) - 0.5f
                );
            }
        }
        else
        {
            roundedPos = pos;
        }

        panddingObject.transform.position = roundedPos;
        if (panddingObject.name == "SoilBox")
        {
            panddingObject.transform.Find("model").transform.position = new Vector3(roundedPos.x, pos.y, roundedPos.z);
        }
        BlockUnbuilding(panddingObject.transform);
    }

    private void UpdateRaycastHit()
    {
        Vector3 origin = UsePlant.Instance.transform.position;
        Vector3 direction = UsePlant.Instance.transform.forward;

        if (Physics.Raycast(origin, direction, out hit, rayLength, layerMask))
        {
            pos = hit.point;
        }

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //if (Physics.Raycast(ray, out hit, 50, layerMask))
        //{
        //    pos = hit.point;
        //}
        

    }

    public void PlaceObject()
    {
        if (canBuilding)
        {
            if (panddingObject?.activeSelf == true)
            {
                if (objects.name == "SoilBox")
                {
                    HowBuilding(objects.transform.Find("model").transform.GetChild(0).GetComponent<Renderer>().sharedMaterial,
                    true, panddingObject.transform);
                }
                else
                {
                    HowBuilding(objects.transform.Find("model").GetComponent<Renderer>().sharedMaterial,
                    true, panddingObject.transform);
                }

                panddingObject.GetComponent<Collider>().enabled = true;

                foreach (Transform ob in panddingObject.transform)
                {
                    if (ob.GetComponent<Collider>() != null)
                    {
                        ob.GetComponent<Collider>().enabled = true;
                        ob.GetComponent<Collider>().tag = "block";
                    }
                }
                panddingObject.GetComponent<Collider>().enabled = true;
                panddingObject.tag = "block";
                panddingObject = null;
                Destroy(panddingObject);
                _isHolding = true;
            }
        }
    }

    public void RotateObject()
    {
        panddingObject.transform.Rotate(Vector3.up, RotateAmount);
    }

    public void SelecObject(Item item)
    {
        Item = item;
        if (panddingObject?.activeSelf == false)
        {
            panddingObject?.SetActive(true);
        }

        if (item.id != ItemID)
        {
            Debug.Log(item.id);
            Destroy(panddingObject);
            CreateNewObjects(item.ObjectOnWorld);
            _isHolding = false;
        }
        else if (_isHolding)
        {
            //Debug.Log("isHold");
            CreateNewObjects(item.ObjectOnWorld);
            _isHolding = false;
        }
        
        ItemID = item.id;
    }

    private void CreateNewObjects(GameObject Ob)
    {
        panddingObject = Instantiate(Ob, pos, Quaternion.identity);
        panddingObject.name = Ob.name;
        objects = Ob;
        HowBuilding(canBuild, true, panddingObject.transform);
    }

    public void CancelObject()
    {
        //Debug.Log(panddingObject == null);
        if (panddingObject != null)
        {
            panddingObject.SetActive(false);
        }
    }

    private float RoundToNearestGrid(float pos)
    {
        float xDiff = pos % gridSize;
        pos -= xDiff;
        

        if (xDiff > (gridSize / 2))
        {
            pos += gridSize;
        }
        return pos;
    }
    private void BlockUnbuilding(Transform BulidObject)
    {
        Collider[] colliders = Physics.OverlapBox(BulidObject.position, BulidObject.localScale / scaleing, BulidObject.rotation, layerMask);
        foreach (Collider collider in colliders)
        {
            if (BulidObject.gameObject.name == "SoilBox")
            {
                if (collider.CompareTag("World"))
                {
                    HowBuilding(canBuild, true, panddingObject.transform);
                }else
                {
                    HowBuilding(cantBuild, false, panddingObject.transform);
                }
            }
            else
            {
                if (collider.CompareTag("block"))
                {
                    HowBuilding(cantBuild, false, panddingObject.transform);
                }
                else
                {
                    HowBuilding(canBuild, true, panddingObject.transform);
                }
            }
        }
        if (BulidObject.gameObject.name == "SoilBox")
        {
            OnBox = false;
        }
        if (OnBox)
        {
            HowBuilding(canBuild, true, panddingObject.transform);
        }
        else if(colliders.Length < 1)
        {
            HowBuilding(cantBuild, false, panddingObject.transform);
        }
    }

    private void HowBuilding(Material material, bool canBuild, Transform ParentTransform)
    {
        canBuilding = canBuild;

        Transform trf = ParentTransform.transform.GetChild(0);
        if (trf.childCount <= 0)
        {
            trf.GetComponent<Renderer>().sharedMaterial = material;
        }
        else
        {
            trf.GetChild(0).GetComponent<Renderer>().sharedMaterial = material;

        }
    }

    public void DestroyTagetOB()
    {
        bool isDestroyOB = true;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 50, layerMask))
        {
            if (hit.collider.CompareTag("block"))
            {
                if (hit.collider.transform.parent == null)
                {
                    Debug.Log(hit.collider.gameObject.name + " : " + isDestroyOB);
                    if (isDestroyOB)
                    {
                        ItemContain item = hit.collider.transform.GetComponent<ItemContain>();
                        InventoryManager.Instance.Add(item.item);
                        Destroy(hit.collider.gameObject);
                        isDestroyOB = false;
                        return;
                    }
                }else
                {
                    Debug.Log(hit.collider.gameObject.name + " : " + isDestroyOB);
                    if (isDestroyOB)
                    {
                        ItemContain item = hit.collider.transform.parent.gameObject.GetComponent<ItemContain>();
                        InventoryManager.Instance.Add(item.item);
                        Destroy(hit.collider.transform.parent.gameObject);
                        isDestroyOB = false;
                        return;
                    }
                }
                
            }
        }
    }
}

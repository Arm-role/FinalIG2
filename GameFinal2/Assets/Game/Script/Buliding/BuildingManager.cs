using System;
using UnityEngine;
using static UnityEditor.Progress;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;
    public float scaleing;
    public GameObject objects;
    public Material canBuild;
    public Material cantBuild;

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
    public bool canBuilding = true;

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
                    Debug.Log("Left");
                    roundedPos = new Vector3(
                    RoundToNearestGrid(pos.x) - distance.x,
                    RoundToNearestGrid(pos.y),
                    RoundToNearestGrid(pos.z) - 0.5f
                    );
                }

                else if (distance.x == 0.5f)
                {
                    Debug.Log("Right");
                    roundedPos = new Vector3(
                    RoundToNearestGrid(pos.x) - distance.x,
                    RoundToNearestGrid(pos.y),
                    RoundToNearestGrid(pos.z) - 0.5f
                    );
                }

                else if (distance.z == -0.5f)
                {
                    Debug.Log("Back");
                    roundedPos = new Vector3(
                    RoundToNearestGrid(pos.x) - 0.5f,
                    RoundToNearestGrid(pos.y),
                    RoundToNearestGrid(pos.z) - distance.z
                    );
                }

                else if (distance.z == 0.5f)
                {
                    Debug.Log("front");
                    roundedPos = new Vector3(
                    RoundToNearestGrid(pos.x) - 0.5f,
                    RoundToNearestGrid(pos.y),
                    RoundToNearestGrid(pos.z) - distance.z
                    );
                }

                else if (distance.y == -0.5f)
                {
                    Debug.Log("Up");
                    roundedPos = new Vector3(
                    RoundToNearestGrid(pos.x) - 0.5f,
                    RoundToNearestGrid(pos.y) - distance.y * 2,
                    RoundToNearestGrid(pos.z) - 0.5f
                    );
                }

                else if (distance.y == 0.5f)
                {
                    Debug.Log("Down");
                    roundedPos = new Vector3(
                    RoundToNearestGrid(pos.x) - 0.5f,
                    RoundToNearestGrid(pos.y) - distance.y * 2,
                    RoundToNearestGrid(pos.z) - 0.5f
                    );
                }else
                {
                    Debug.Log(distance);
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
            Debug.Log("SnapToWorld");
        }
        BlockUnbuilding(panddingObject.transform);
    }

    private void UpdateRaycastHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 50, layerMask))
        {
            pos = hit.point;
        }
    }

    public void PlaceObject()
    {
        if (canBuilding)
        {
            if (panddingObject?.activeSelf == true)
            {
                panddingObject.transform.Find("model").GetComponent<Renderer>().sharedMaterial = 
                objects.transform.Find("model").GetComponent<Renderer>().sharedMaterial;

                panddingObject.GetComponent<Collider>().enabled = true;
                if (panddingObject.transform.Find("model").GetComponent<Collider>() != null)
                {
                    Collider coll = panddingObject.transform.Find("model").GetComponent<Collider>();
                    coll.enabled = true;
                    panddingObject.transform.Find("model").tag = "block";
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
        panddingObject.transform.Find("model").GetComponent<Renderer>().sharedMaterial = canBuild;
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
            if (collider.CompareTag("block"))
            {
                canBuilding = false;
                panddingObject.transform.Find("model").GetComponent<Renderer>().sharedMaterial = cantBuild;
            }
            else
            {
                canBuilding = true;
                panddingObject.transform.Find("model").GetComponent<Renderer>().sharedMaterial = canBuild;
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(cube.transform.position, cube.transform.localScale / scaleing);
    }
}

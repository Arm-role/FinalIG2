using UnityEngine;

public class GrowSystem : MonoBehaviour
{
    private GameObject ObID;

    public int Min;
    public int Max;
    public float timer = 0;
    public float timeSpeed;
    public bool HavePlant = false;

    private GameObject plantOB;
    private GameObject ParentPlant;

    private bool isCreate = true;
    public bool isGrow = true;

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
                    case < 100:
                        if (isCreate)
                        {
                            //Debug.Log("State1");
                            CreateOB(ObID.transform.GetChild(0).gameObject);
                            isCreate = false;

                            if (ParticleManager.instance.PlantGrow != null)
                            {
                                GameObject hit = Instantiate(ParticleManager.instance.PlantGrow.gameObject, transform.position, transform.rotation);
                                Destroy(hit, 2f);
                            }
                        }
                        break;
                    case < 101:
                        isCreate = true;
                        break;
                    case < 200:
                        if (isCreate)
                        {
                            //Debug.Log("State2");
                            Destroy(plantOB);
                            CreateOB(ObID.transform.GetChild(1).gameObject);
                            isCreate = false;

                            if (ParticleManager.instance.PlantGrow != null)
                            {
                                GameObject hit = Instantiate(ParticleManager.instance.PlantGrow.gameObject, transform.position, transform.rotation);
                                Destroy(hit, 2f);
                            }
                        }
                        break;
                    case < 201:
                        isCreate = true;
                        break;
                    case > 202:
                        isCreate = true;
                        if (isCreate)
                        {
                            //Debug.Log("State3");
                            Destroy(plantOB);
                            CreateOB(ObID.transform.GetChild(2).gameObject);
                            isCreate = false;
                            isGrow = false;

                            if (ParticleManager.instance.PlantGrow != null)
                            {
                                GameObject hit = Instantiate(ParticleManager.instance.PlantGrow.gameObject, transform.position, transform.rotation);
                                Destroy(hit, 2f);
                            }
                        }
                        break;
                }
            }
        }
    }
    private void CreateOB(GameObject Ob)
    {
        if (ParentPlant == null)
        {
            ParentPlant = new GameObject("parentPlant");
            ParentPlant.tag = "Plant";
            ParentPlant.transform.SetParent(transform);
            ParentPlant.transform.position = transform.position;
        }
        plantOB = Instantiate(Ob, transform);
        plantOB.transform.position = transform.GetChild(0).position;
        plantOB.name = "Plant";
    }
    public void GetSeed(Item item)
    {
        this.item = item;
        ObID = item.ObjectOnWorld;
        HavePlant = true;
    }
    public void Havest()
    {
        if (!isGrow)
        {
            if(item != null)
            {
                RateDropItem();
                Destroy(ParentPlant);
                Destroy(plantOB);
                isGrow = true;
                isCreate = true;
                plantOB = null;
                ObID = null;
                timer = 0;
            }
        }
    }
    private void RateDropItem()
    {
        for (int i = 0; i < Random.Range(Min, Max); i++)
        {
            InventoryManager.Instance.Add(item);
        }
    }
}

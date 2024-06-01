using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;

public class SpawnRaid : MonoBehaviour
{
    public static SpawnRaid instance;
    
    public GameObject RaidOB;
    public TextMeshProUGUI TextTimer;
    public Enemy[] enemies;
    public Transform[] spawner;

    public List<(Transform, List<Transform>)> TagetGroup = new List<(Transform, List<Transform>)>();

    private List<List<Transform>> groupInput;
    private NavMeshSurface surface;
    bool iscreate = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    private void Start()
    {
        surface = GetComponent<NavMeshSurface>();
    }
    private void Update()
    {
        if (iscreate)
        {
            manageGroup();
            iscreate = false;
        }

    }
    public void manageGroup()
    {
        TagetGroup.Clear();
        if (transform.childCount > 0)
        {
            for(int i = 0; i < groupInput.Count; i++)
            {
                TagetGroup.Add((transform.GetChild(i), groupInput[i]));
            }
            Raid();
        }
    }
    public void Raid()
    {
        List<GameObject> EnemyList = new List<GameObject>();

        foreach (Transform tran in spawner)
        {
            int ranEn = Random.Range(0, enemies.Length);
            int ranRate = Random.Range(1, enemies[ranEn].rateSpawn);
            for (int i = 0; i < ranRate; i++)
            {
                EnemyList.Add(enemies[ranEn].Prefab);
            }
            if (EnemyList.Count > 0)
            {
                foreach (GameObject enOB in EnemyList)
                {
                    GameObject EnemOB = Instantiate(enOB, tran);
                    var tagetData = EnemOB.GetComponent<TagetData>();
                    tagetData.TagetGroup = TagetGroup;
                    StartCoroutine(creatCooldown(1));
                }
            }
        }
    }
    public void GroupInput(List<List<Transform>> groups)
    {
        if (groupInput == null)
        {
            Debug.Log("null");
            groupInput = groups;
            iscreate = true;

        }
        else
        {
            Debug.Log("Unnull");
            groupInput.Clear();
            groupInput = groups;
            iscreate = true;
        }

    }
    public void TimeWorld(float timer)
    {
        int minutes = Mathf.FloorToInt(timer / 60);  // คำนวณนาที
        int seconds = Mathf.FloorToInt(timer % 60);  // คำนวณวินาที

        TextTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void CreateRaidOB(Transform gropTransform)
    {
        GameObject ob = Instantiate(RaidOB, transform);
        ob.transform.position = new Vector3(gropTransform.position.x, gropTransform.position.y + 2, gropTransform.position.z);
    }
    public void DestroyRaidOB()
    {
        if (transform.childCount != 0)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
    public void ReNavMesh()
    {
        surface.RemoveData();
        surface.BuildNavMesh();
    }

    IEnumerator creatCooldown(float time)
    {
        Debug.Log("Create");
        yield return new WaitForSeconds(time);
    }
}

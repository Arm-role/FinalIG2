using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnRaid : MonoBehaviour
{
    public static SpawnRaid instance;
    
    public GameObject RaidOB;
    public TextMeshProUGUI TextTimer;

    public List<List<Transform>> RaidGroup = new List<List<Transform>>();

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

    //public void inputGroup(List<List<Transform>> group)
    //{
    //    RaidGroup.Clear();
    //    RaidGroup = group;

    //    for (int i = 0; i < RaidGroup.Count; i++)
    //    {
    //        CreateRaidOB(RaidGroup[i][0].transform, i);
    //    }
    //    foreach (List<Transform> groupRaid in RaidGroup)
    //    {

    //    }
    //}
    public void TimeWorld(float timer)
    {
        int minutes = Mathf.FloorToInt(timer / 60);  // คำนวณนาที
        int seconds = Mathf.FloorToInt(timer % 60);  // คำนวณวินาที

        TextTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void GroupInput(List<List<Transform>> groups)
    {
        RaidGroup.Clear();
        RaidGroup = groups;
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
            Debug.Log("3");
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}

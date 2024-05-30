using System.Collections.Generic;
using UnityEngine;

public class SpawnRaid : MonoBehaviour
{
    public static SpawnRaid instance;
    
    public GameObject RaidOB;
    public List<List<Transform>> RaidGroup = new List<List<Transform>>();

    private List<GameObject> RaidList = new List<GameObject>();
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
    
    public void inputGroup(List<List<Transform>> group)
    {
        RaidGroup.Clear();
        RaidGroup = group;

        for (int i = 0; i < RaidGroup.Count; i++)
        {
            CreateRaidOB(RaidGroup[i][0].transform, i);
        }
        foreach (List<Transform> groupRaid in RaidGroup)
        {
        }
    }
    public void CreateRaidOB(Transform transform, int i)
    {

    }
}

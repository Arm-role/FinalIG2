using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class ChackFarm : MonoBehaviour
{
    public static ChackFarm instance;
    public float threshold = 5f;
    public List<Transform> transforms = new List<Transform>();

    public bool FindOB = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.L))
        //{
        //    FindOP();
        //}

        GameObject[] Plants = GameObject.FindGameObjectsWithTag("Plant");
        List<Transform> Transforms = new List<Transform>();
        if (Plants.Length > 0)
        {
            for (int i = 0; i < Plants.Length; i++)
            {
                Transforms.Add(Plants[i].transform);
            }
        }
        if (Transforms.Count != transforms.Count)
        {
            FindOP();
        }else
        {
            FindOB = false;
        }
    }
    public void FindOP()
    {
        FindOB = true;
        SpawnRaid.instance.DestroyRaidOB();
        transforms?.Clear();

        GameObject[] Plants = GameObject.FindGameObjectsWithTag("Plant");

        if (Plants.Length > 0)
        {
            for (int i = 0;i < Plants.Length;i++)
            {
                transforms.Add(Plants[i].transform);
            }
        }

        if (transforms.Count > 0)
        {
            List<List<Transform>> groups = GroupTransformsByDistance(transforms.ToArray(), threshold);
            SpawnRaid.instance.GroupInput(GroupTransformsByDistance(transforms.ToArray(), threshold));

            for (int i = 0; i < groups.Count; i++)
            {
                SpawnRaid.instance.CreateRaidOB(groups[i][0].transform);
            }
        }
    }
    List<List<Transform>> GroupTransformsByDistance(Transform[] transforms, float threshold)
    {
        List<List<Transform>> groups = new List<List<Transform>>();
        HashSet<Transform> visited = new HashSet<Transform>();

        foreach (Transform t in transforms)
        {
            if (!visited.Contains(t))
            {
                List<Transform> group = new List<Transform>();
                FindNearbyTransforms(t, transforms, threshold, group, visited);
                groups.Add(group);
            }
        }

        return groups;
    }
    void FindNearbyTransforms(Transform current, Transform[] transforms, float threshold, List<Transform> group, HashSet<Transform> visited)
    {
        group.Add(current);
        visited.Add(current);

        foreach (Transform t in transforms)
        {
            if (!visited.Contains(t) && Vector3.Distance(current.position, t.position) <= threshold)
            {
                FindNearbyTransforms(t, transforms, threshold, group, visited);
            }
        }
    }
}

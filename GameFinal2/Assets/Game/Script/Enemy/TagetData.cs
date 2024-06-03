using StarterAssets;
using System.Collections.Generic;
using UnityEngine;

public class TagetData : MonoBehaviour
{
    public List<(Transform, List<Transform>)> TagetGroup;
    public List<(Transform, List<Transform>)> InstanTagetGroup;
    public List<Transform> Enemylist;
    public Transform target;

    private AntAI AntAI;

    private void Start()
    {
        AntAI = GetComponent<AntAI>();

        TagetGroup = SpawnRaid.instance.TagetGroup;

        ReEnemylist();
        SortObjectsByDistance();
    }

    private void Update()
    {
        TagetGroup = SpawnRaid.instance.TagetGroup;
        AntAI.AddTaget(target);

        if(ChackFarm.instance.FindOB)
        {
            ReEnemylist();
            SortObjectsByDistance();
        }
        if (Enemylist.Count > 0)
        {
            target = Enemylist[0];
        }
        else
        {
            target = ThirdPersonController.instance.PlayerTransform; 
        }
    }
    private void ReEnemylist()
    {
        Enemylist?.Clear();
        if (TagetGroup != null)
        {
            foreach (var t in TagetGroup)
            {
                if (FindClosest(TagetGroup) == t.Item1)
                {
                    foreach (Transform tran in t.Item2)
                    {
                        if (tran != null)
                        {
                            Enemylist.Add(tran.parent);
                        }
                    }
                }
            }
        }
    }
    Transform FindClosest(List<(Transform, List<Transform>)> objects)
    {
        Transform closest = null;
        float closestDistanceSqr = Mathf.Infinity; // กำหนดค่าเริ่มต้นเป็น Infinity
        Vector3 currentPosition = transform.position;

        foreach (var obj in objects)
        {
            Vector3 directionToTarget = obj.Item1.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude; // ใช้ sqrMagnitude แทน Magnitude เพื่อประสิทธิภาพที่ดีขึ้น

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closest = obj.Item1;
            }
        }
        return closest;
    }
    void SortObjectsByDistance()
    {
        Enemylist.Sort((obj1, obj2) =>
        {
            float distanceToObj1 = Vector3.Distance(obj1.position, transform.position);
            float distanceToObj2 = Vector3.Distance(obj2.position, transform.position);
            return distanceToObj1.CompareTo(distanceToObj2);
        });
    }
}

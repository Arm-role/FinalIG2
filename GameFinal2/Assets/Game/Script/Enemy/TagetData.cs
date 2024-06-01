using System.Collections.Generic;
using UnityEngine;

public class TagetData : MonoBehaviour
{
    public List<(Transform, List<Transform>)> TagetGroup;
    public List<Transform> Enemylist;
    public Transform target;

    private AntAI AntAI;
    List<Transform> trans = new List<Transform>();

    bool isKeepEnemy = false;
    private void Start()
    {
        AntAI = GetComponent<AntAI>();
        foreach (var t in TagetGroup)
        {
            trans.Add(t.Item1);
        }
    }

    private void Update()
    {
        target = FindClosest(trans.ToArray());

        if (target != null)
        {
            if (!isKeepEnemy)
            {
                foreach (var t in TagetGroup)
                {
                    if (target == t.Item1)
                    {
                        foreach (Transform tran in t.Item2)
                        {
                            Enemylist.Add(tran.parent);
                        }
                    }
                }
                isKeepEnemy = true;
            }
            AntAI.Taget = target;
            Debug.Log(target.name);
        }
        else
        {
            Debug.Log("null");
        }
    }
    Transform FindClosest(Transform[] objects)
    {
        Transform closest = null;
        float closestDistanceSqr = Mathf.Infinity; // กำหนดค่าเริ่มต้นเป็น Infinity
        Vector3 currentPosition = transform.position;

        foreach (Transform obj in objects)
        {
            Vector3 directionToTarget = obj.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude; // ใช้ sqrMagnitude แทน Magnitude เพื่อประสิทธิภาพที่ดีขึ้น

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closest = obj;
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

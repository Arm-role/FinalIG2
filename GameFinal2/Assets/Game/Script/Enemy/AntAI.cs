using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class AntAI : MonoBehaviour
{
    public Transform Taget;

    public bool velocity;
    public bool desiredVelocity;
    public bool path;

    public List<Transform> Enemylist;

    private NavMeshAgent agent;
    private Animator animator;

    int enemyNum = 0;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Taget != null)
        {
            if(enemyNum < Enemylist.Count)
            {
                agent.destination = Enemylist[enemyNum].position;
                float targetDistance = Vector3.Distance(transform.position, agent.destination);
                if (targetDistance <= 1f)
                {
                    StartCoroutine(changeEnemy(2));
                    Debug.Log(enemyNum);
                }
            }
        }
        else
        {
            agent.destination = transform.position;
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            enemyNum++;
        }
        //Debug.Log(Vector3.Distance(transform.position, agent.destination));
    }
    public void AddEnemylist(List<Transform> list)
    {
        if (Enemylist == null)
        {
            Enemylist = list;
            SortObjectsByDistance();

        }
        else
        {
            Enemylist.Clear();
            Enemylist = list;
            SortObjectsByDistance();

        }
    }
    public void AddTaget(Transform target)
    {
        Taget = target;
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
    private void OnDrawGizmos()
    {
        if (velocity)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + agent.velocity);
        }
        if (desiredVelocity)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + agent.desiredVelocity);
        }
        if(path)
        {
            Gizmos.color = Color.black;
            var agentPath = agent.path;
            Vector3 prevCorner = transform.position;
            foreach (var Corner in agentPath.corners)
            {
                Gizmos.DrawLine(prevCorner, Corner);
                Gizmos.DrawSphere(Corner, 0.1f);
                prevCorner = Corner;
            }
        }
    }

    IEnumerator changeEnemy(float timer)
    {
        yield return new WaitForSeconds(timer);
    }
}

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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(Taget != null)
        {
            agent.destination = Taget.position;
        }
        else
        {
            agent.destination = transform.position;
        }
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
}

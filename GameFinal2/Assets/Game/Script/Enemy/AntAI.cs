using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class AntAI : MonoBehaviour
{
    public Transform Taget;
    public float Damage;
    public float Attackcoldown;
    public float SphereRadien;
    public Vector3 SpherOffset;

    public bool velocity;
    public bool desiredVelocity;
    public bool path;

    private NavMeshAgent agent;
    private Animator animator;

    bool canAttack = true;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            agent.isStopped = true;
        }
        if (Taget != null &&  agent.isOnNavMesh)
        {
            agent.SetDestination(Taget.position);

            if (agent.remainingDistance <= 0.5f && !agent.pathPending)
            {
                
            }
            Collider[] coll = Physics.OverlapSphere(transform.position + SpherOffset, SphereRadien);
            if (coll.Length > 0)
            {
                foreach (Collider collider in coll)
                {
                    if (collider.TryGetComponent<BlockHeath>(out BlockHeath heath1))
                    {
                        Debug.Log(collider);
                        BlockHeath heath2 = Taget.GetComponent<BlockHeath>();
                        if (heath2 == heath1)
                        {
                            //Debug.Log("attackPlant");
                            Attack(heath2);
                        }
                        else if (collider.CompareTag("block"))
                        {
                            //Debug.Log("attackBlock");
                            Attack(heath1);
                        }
                    }
                    if (collider.TryGetComponent<Health>(out Health heath))
                    {
                        if (collider.CompareTag("Player"))
                        {
                            Attack(heath);
                        }
                    }
                }
            }
        }
        else
        {
            agent.SetDestination(transform.position);
        }
    }
    public void AddTaget(Transform target)
    {
        Taget = target;
    }
    private void Attack(BlockHeath health)
    {
        if (canAttack)
        {
            animator.SetTrigger("Attack");
            health.TakeDamage(Damage);
            StartCoroutine(TakeDamage(Attackcoldown));
        }
    }
    private void Attack(Health health)
    {
        if (canAttack)
        {
            animator.SetTrigger("Attack");
            health.TakeDamage(Damage);
            StartCoroutine(TakeDamage(Attackcoldown));
        }
    }
    IEnumerator TakeDamage(float timer)
    {
        canAttack = false;
        agent.isStopped = true;
        yield return new WaitForSeconds(timer);
        canAttack = true;
        agent.isStopped = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + SpherOffset, SphereRadien);
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

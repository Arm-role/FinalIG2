using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDestructible : MonoBehaviour
{
    [SerializeField]
    private int DestructibleObjectCheckRate = 10;
    [SerializeField]
    private float CheckDistance = 1f;
    [SerializeField]
    private NavMeshAgent Agent;
    [SerializeField]
    private float DestructibleAttackDelay = 1f;
    [SerializeField]
    private int DestructibleAttackDamage = 10;
    [SerializeField]
    private LayerMask DestructibleLayer;

    private EnemyCurrentState enemy;
    private NavMeshPath OriginPath;

    private Coroutine Checkcoroutine;
    private Coroutine AttackCoroutune;

    private void Awake()
    {
        enemy = GetComponent<EnemyCurrentState>();
        enemy.OnStateChange += HandleStateChange;
    }
    private void HandleStateChange(EnemyState oldState, EnemyState newState)
    {
        if (newState == EnemyState.Chase)
        {
            if (Checkcoroutine != null)
            {
                StopCoroutine(Checkcoroutine);
            }
            if (AttackCoroutune != null)
            {
                StopCoroutine(AttackCoroutune);
            }
            Checkcoroutine = StartCoroutine(CheckforDestruct());
        }
    }
    IEnumerator CheckforDestruct()
    {
        yield return null;
        WaitForSeconds wait = new WaitForSeconds(1f / DestructibleObjectCheckRate);
        Vector3[] corners = new Vector3[2];

        bool foundDestructible = false;
        while (!foundDestructible)
        {
            int length = Agent.path.GetCornersNonAlloc(corners);
            Debug.Log(length);
            if (length > 1)
            {
                Debug.DrawRay(corners[0], (corners[1] - corners[0]).normalized, Color.yellow);
                Debug.Log("Ray");
                if (Physics.Raycast(corners[0],(corners[1] - corners[0]).normalized,out RaycastHit hit,
                    CheckDistance,
                    DestructibleLayer
                    ))
                {
                    Debug.Log(hit.collider.transform.name);
                    if (hit.collider.TryGetComponent<BlockHeath>(out BlockHeath destructible))
                    {
                        Debug.Log("destroy2");
                        destructible.onDsetroy += HandleDestroy;
                        OriginPath = Agent.path;
                        Agent.enabled = false;
                        enemy.ChangeState(EnemyState.Destroy);
                        StopCoroutine(Checkcoroutine);
                        AttackCoroutune = StartCoroutine(AttackDestructible(destructible));
                        foundDestructible = true;
                        break;
                    }
                }
            }
            yield return wait;
        }
    }
    private void HandleDestroy()
    {
        if (AttackCoroutune != null)
        {
            StopCoroutine(AttackCoroutune);
        }

        if (enemy.state == EnemyState.Destroy)
        {
            Agent.enabled = true;
            Agent.SetPath(OriginPath);
            enemy.ChangeState(EnemyState.Chase);
        }

    }
    IEnumerator AttackDestructible(BlockHeath destructible)
    {
        WaitForSeconds wait = new WaitForSeconds(DestructibleAttackDelay);
        while (destructible != null)
        {
            destructible.TakeDamage(DestructibleAttackDamage);

            yield return wait;
        }
    }
}

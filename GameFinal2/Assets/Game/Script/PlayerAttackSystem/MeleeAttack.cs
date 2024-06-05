using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public static MeleeAttack instance;
    public Vector3 Offset;
    public Transform Target;
    public float Radien;

    
    bool isAttack = false;

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
    void Update()
    {
        if (isAttack)
        {
            Vector3 origin = Target.position - Offset;
            Collider[] colliders = Physics.OverlapSphere(origin, Radien);
            foreach (Collider collider in colliders)
            {
                if (!collider.isTrigger)
                {
                    if (collider.CompareTag("Enemy"))
                    {
                        if (collider.transform.TryGetComponent<Health>(out Health health))
                        {
                            if (ParticleManager.instance.HitMelee != null)
                            {
                                GameObject hit = Instantiate(ParticleManager.instance.HitMelee.gameObject, Target.position - Offset, transform.rotation);
                                Destroy(hit, 2f);
                            }

                            PlayerAttack.Instance.Attack(health);
                            isAttack = false ;
                            break;
                        }
                    }
                }
            }
        }
    }
    public void Attacking()
    {
        isAttack = true;
    }
    public void EndAttack()
    {
        isAttack = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Target.position - Offset, Radien);
    }
}

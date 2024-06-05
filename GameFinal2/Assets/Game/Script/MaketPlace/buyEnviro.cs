using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class buyEnviro : MonoBehaviour
{
    public Vector3 Offset;
    public float Radien;
    public float rotationSpeed;
    public Transform canvas;
    [SerializeField] private int Cost;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    public Transform cameratf;

    private Animator animator;

    bool isBuy = false;
    bool isTrig = true;
    void Start()
    {
        animator = GetComponent<Animator>();
        cameratf = Camera.main.transform;
        textMeshPro.text = Cost.ToString();
    }

    void Update()
    {
        Vector3 origin = transform.position - Offset;
        Collider[] colliders = Physics.OverlapSphere(origin, Radien);
        List<Collider> coll = new List<Collider>();
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                coll.Add(collider);
            }
        }
        if (coll.Count > 0)
        {
            canvas.gameObject?.SetActive(true);

            Vector3 direction = cameratf.transform.position - canvas.transform.position;
            Quaternion rotation = Quaternion.LookRotation(-direction);
            canvas.transform.rotation = Quaternion.Slerp(canvas.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            isBuy = true;
        }
        else
        {
            isBuy = false;
            isTrig = false;
            canvas.gameObject?.SetActive(false);
        }

        if (isBuy)
        {
            if (!isTrig)
            {
                animator.SetTrigger("fade");
                isTrig = true;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                BuyItem();
            }
        }
    }

    void BuyItem()
    {
        if (MyMoneySystem.instance.Money >= Cost)
        {
            if ((MyMoneySystem.instance.Money - Cost) >= 0)
            {
                MyMoneySystem.instance.Money -= Cost;
                MyMoneySystem.instance.ShowOnInventory();

                if (ParticleManager.instance.BockDestroy != null)
                {
                    GameObject particle = Instantiate(ParticleManager.instance.BockDestroy.gameObject, transform.position, transform.rotation);
                    Destroy(particle, 2f);
                    Destroy(gameObject);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position - Offset, Radien);
    }
}

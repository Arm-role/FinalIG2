using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class BuyAndSellArea : MonoBehaviour
{
    public Vector3 offset;
    public int showAdvice;
    public LayerMask showAdviceMask;

    public Vector3 scaleBox;
    public float radius = 5;
    public bool isBox = false;

    bool isBuy = false;
    void Update()
    {
        if (isBox)
        {
            DrawOverlapBox();
        }else
        {
            DrawOverlapSphere();
        }
        ShowAdvInput(showAdvice);
    }
    private void DrawOverlapSphere()
    {
        Vector3 origin = transform.position + offset;

        Collider[] Collider = Physics.OverlapSphere(origin, radius, showAdviceMask);

        List<Collider> coll = new List<Collider>();
        foreach (Collider hit in Collider)
        {
            if (hit.GetComponent<Collider>().CompareTag("Player"))
            {
                coll.Add(hit);
            }
        }
        if (coll.Count > 0)
        {
            showAdvice = 1;
        }
        else
        {
            showAdvice = 0;
        }
    }
    private void DrawOverlapBox()
    {
        Vector3 origin = transform.position + offset;

        Collider[] Collider = Physics.OverlapBox(origin, scaleBox, Quaternion.identity, showAdviceMask);

        List<Collider> coll = new List<Collider>();
        foreach (Collider hit in Collider)
        {
            if (hit.GetComponent<Collider>().CompareTag("Player"))
            {
                coll.Add(hit);
            }
        }
        if (coll.Count > 0)
        {
            showAdvice = 2;
        }
        else
        {
            showAdvice = 0;
        }
    }
    private void ShowAdvInput(int adv)
    {
        if (Advice.Instance.showAdvice != adv && adv != 0)
        {
            Advice.Instance.showAdvice = adv;
            isBuy = false;
        }
        else if(adv == 0)
        {
            if(!isBuy)
            {
                Advice.Instance.showAdvice = adv;
                isBuy = true;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position + offset;
        Gizmos.color = Color.yellow;
        
        if (isBox)
        {
            Gizmos.DrawWireCube(origin, scaleBox);
        }else
        {
            Gizmos.DrawWireSphere(origin, radius);
        }
    }
}


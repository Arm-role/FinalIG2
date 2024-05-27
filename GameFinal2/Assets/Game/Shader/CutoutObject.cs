using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private LayerMask WallMask;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
    void Update()
    {
        Vector2 cutOutPos = cam.WorldToViewportPoint(target.position);
        cutOutPos.y = Screen.height / Screen.width;

        Vector3 offset = target.position - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, WallMask);

        for (int i = 0; i < hitObjects.Length; i++)
        {
            if (hitObjects[i].transform.GetComponent<Renderer>().materials != null)
            {
                Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;

                for (int j = 0; j < materials.Length; j++)
                {
                    materials[j].SetVector("_Cutout_Position", cutOutPos);
                    materials[j].SetFloat("_Cutout_Size", 0.1f);
                    materials[j].SetFloat("_Falloff_Size", 0.05f);
                }
            }
        }
    }
}

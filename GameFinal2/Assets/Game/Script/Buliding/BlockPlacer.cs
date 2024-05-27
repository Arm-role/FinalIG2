using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    public GameObject blockPrefab; // Prefab ของบล็อกที่จะวาง
    private MinecraftBuildSystem gridSystem;

    void Start()
    {
        gridSystem = FindObjectOfType<MinecraftBuildSystem>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ตรวจจับการคลิกซ้ายของเมาส์
        {
            PlaceBlock();
        }
    }

    void PlaceBlock()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 point = hit.point;
            Vector3 gridPoint = gridSystem.GetNearestPointOnGrid(point);
            Instantiate(blockPrefab, gridPoint, Quaternion.identity);
        }
    }
}

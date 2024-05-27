using UnityEngine;

public class MinecraftBuildSystem : MonoBehaviour
{
    public float gridSize = 1.0f; // ขนาดของแต่ละช่องในกริด

    // ฟังก์ชันนี้จะใช้คำนวณตำแหน่งในกริดที่ใกล้ที่สุด
    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x / gridSize);
        int yCount = Mathf.RoundToInt(position.y / gridSize);
        int zCount = Mathf.RoundToInt(position.z / gridSize);

        Vector3 result = new Vector3(
            (float)xCount * gridSize,
            (float)yCount * gridSize,
            (float)zCount * gridSize);

        result += transform.position;

        return result;
    }

    // ฟังก์ชันนี้จะแสดงจุดในกริดใน Scene view สำหรับการดีบัก
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (float x = -5; x < 5; x += gridSize)
        {
            for (float y = -5; y < 5; y += gridSize)
            {
                for (float z = -5; z < 5; z += gridSize)
                {
                    var point = GetNearestPointOnGrid(new Vector3(x, y, z));
                    Gizmos.DrawSphere(point, 0.1f);
                }
            }
        }
    }
}

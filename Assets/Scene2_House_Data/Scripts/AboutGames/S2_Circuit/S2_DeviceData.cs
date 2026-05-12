using UnityEngine;

public class S2_DeviceData : MonoBehaviour
{
    [Header("Device Info")]
    public S2_DeviceType deviceType;

    [Header("First Nodes")]
    // ลาก Object ที่มีสคริปต์ WireNode มาใส่ในนี้
    public S2_WireNode redNode1; // จุดขั้วบวก (หรือจุดซ้าย)
    public S2_WireNode blackNode1; // จุดขั้วลบ (หรือจุดขวา)

    [Header("Second Nodes")]
    public S2_WireNode redNode2; // จุดขั้วบวก (หรือจุดซ้าย)
    public S2_WireNode blackNode2; // จุดขั้วลบ (หรือจุดขวา)
    // ฟังก์ชันเช็คว่าอุปกรณ์นี้ "เชื่อมต่อครบวงจร" หรือยัง
}
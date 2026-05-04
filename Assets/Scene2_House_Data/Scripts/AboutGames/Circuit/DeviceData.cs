using UnityEngine;

public class DeviceData : MonoBehaviour
{
    [Header("Device Info")]
    public DeviceType deviceType;

    [Header("First Nodes")]
    // ลาก Object ที่มีสคริปต์ WireNode มาใส่ในนี้
    public WireNode redNode1; // จุดขั้วบวก (หรือจุดซ้าย)
    public WireNode blackNode1; // จุดขั้วลบ (หรือจุดขวา)

    [Header("Second Nodes")]
    public WireNode redNode2; // จุดขั้วบวก (หรือจุดซ้าย)
    public WireNode blackNode2; // จุดขั้วลบ (หรือจุดขวา)
    // ฟังก์ชันเช็คว่าอุปกรณ์นี้ "เชื่อมต่อครบวงจร" หรือยัง
}
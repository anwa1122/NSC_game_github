using UnityEngine;

// วางไว้บน block แต่ละอัน
// ไม่จำเป็นต้องมี Collider บน block หลัก เพราะ port จัดการ click เอง
public class BlockNode : MonoBehaviour
{
    [Header("Block Info")]
    public string blockName = "Block";

    // เรียกเมื่อ block นี้ถูก connect กับ block อื่น (override ได้ตามต้องการ)
    public virtual void OnConnected(BlockNode other)
    {
        Debug.Log($"{blockName} connected to {other.blockName}");
    }
}

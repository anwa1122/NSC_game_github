using UnityEngine;

public class CollectibleTrash : MonoBehaviour
{
    public TrashData data; // ไฟล์สีฟ้าที่เก็บข้อมูล

    // --- เพิ่มส่วนนี้เข้าไปครับ ---
    public void Setup(TrashData newData)
    {
        data = newData; // รับข้อมูลจาก Spawner มาเก็บไว้ที่ตัวมันเอง

        // ถ้าในข้อมูลมีโมเดล 3D ให้เสกออกมาโชว์
        if (data.model3D != null)
        {
            GameObject visual = Instantiate(data.model3D, transform.position, transform.rotation);
            visual.transform.SetParent(this.transform); // ให้โมเดลเป็นลูกของ Cube

            // ปิดตัว Cube (MeshRenderer) ให้เหลือแค่ Hitbox
            if (GetComponent<MeshRenderer>() != null)
            {
                GetComponent<MeshRenderer>().enabled = false;
            }
        }

        // เปลี่ยนชื่อ Object ใน Hierarchy ให้ดูง่าย (Optional)
        gameObject.name = "Trash_" + data.trashName;
    }
    // -------------------------

    public void Collect()
    {
        // ส่งข้อมูลไฟล์สีฟ้าในตัวมัน เข้าไปในกระเป๋าตัวกลาง
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(data);
        }

        Destroy(gameObject); // หายตัวไป!
    }
}
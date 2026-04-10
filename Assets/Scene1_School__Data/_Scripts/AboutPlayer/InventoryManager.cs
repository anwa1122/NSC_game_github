using System.Collections.Generic;
using UnityEngine;

// 1. เพิ่ม , IResettable
public class InventoryManager : MonoBehaviour, IResettable
{
    public static InventoryManager Instance;

    public List<TrashData> items = new List<TrashData>();

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // 2. เพิ่มฟังก์ชันรีเซ็ตตามกฎ Interface
    public void ResetObject()
    {
        items.Clear(); // ล้างขยะทั้งหมดในตัวทิ้ง
        Debug.Log("Inventory Cleared for the new day!");
        
        // ถ้าคุณมีระบบ UI Inventory อย่าลืมสั่ง Update UI ให้เป็นค่าว่างตรงนี้ด้วยนะครับ
    }

    public void AddItem(TrashData data)
    {
        items.Add(data);
        Debug.Log("เก็บ " + data.trashName + " แล้ว! ตอนนี้มีขยะ " + items.Count + " ชิ้น");
    }

    public void RemoveItem(TrashData data)
    {
        if (items.Contains(data))
        {
            items.Remove(data);
        }
    }
}
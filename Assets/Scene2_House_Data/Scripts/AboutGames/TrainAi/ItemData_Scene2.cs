using UnityEngine;

// หมวดหมู่หลัก (สุ่มมาทีละหมวด)
public enum ItemCategory_Scene2
{
    Trash,
    Animal
}

// ประเภทย่อยของขยะ
public enum TrashType_Scene2
{
    Recyclable,   // รีไซเคิล
    Organic,      // เศษอาหาร
    Hazardous,    // อันตราย
    General       // ทั่วไป
}

// ประเภทย่อยของสัตว์
public enum AnimalType_Scene2
{
    Dog,
    Cat,
    Pig,
    Chicken
}

// ข้อมูลของไอเทมแต่ละชิ้น
[System.Serializable]
public class ItemData
{
    public string itemName;
    public ItemCategory_Scene2 category;

    // ใช้ int เก็บค่า enum ย่อย เพื่อให้รองรับทั้งสองหมวด
    // ถ้า category == Trash → subType คือ (TrashType)
    // ถ้า category == Animal → subType คือ (AnimalType)
    public int subType;

    public Sprite icon; // ใส่รูปทีหลัง ตอนนี้ปล่อยว่างได้

    // Helper: ดึง subType เป็น TrashType
    public TrashType_Scene2 GetTrashType() => (TrashType_Scene2)subType;

    // Helper: ดึง subType เป็น AnimalType
    public AnimalType_Scene2 GetAnimalType() => (AnimalType_Scene2)subType;
}
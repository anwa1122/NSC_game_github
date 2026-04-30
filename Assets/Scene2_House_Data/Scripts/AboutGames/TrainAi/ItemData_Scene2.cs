using UnityEngine;

// หมวดหมู่หลัก
public enum ItemCategory_Scene2
{
    Trash,
    Animal
}

// ประเภทย่อยของขยะ
public enum TrashType_Scene2
{
    None,
    Glass,   // 0
    Plastic,      // 1
    Metal,    // 2
    Aluminium       // 3
}

// ประเภทย่อยของสัตว์
public enum AnimalType_Scene2
{
    None,
    Dog,          // 0
    Cat,          // 1
    Pig,          // 2
    Chicken       // 3
}

[CreateAssetMenu(fileName = "New Item", menuName = "TrainAI/Item Data")]
public class ItemData_Scene2 : ScriptableObject
{
    public string itemName;
    public ItemCategory_Scene2 category;

    // แยกออกมาเป็น Dropdown ให้เลือกได้เลยใน Unity
    public TrashType_Scene2 trashType;
    public AnimalType_Scene2 animalType;

    public Sprite icon;

    // ปรับ Helper ให้ดึงค่าจากตัวแปรที่เลือกไว้โดยตรง
    public TrashType_Scene2 GetTrashType() => trashType;
    public AnimalType_Scene2 GetAnimalType() => animalType;
}
using UnityEngine;

// ประเภทย่อยของขยะ
public enum TrashType_Scene2
{
    None,
    Glass,   // 0
    Plastic,      // 1
    Metal,    // 2
    Aluminium       // 3
}

[CreateAssetMenu(fileName = "New Item", menuName = "TrainAI/Item Data")]
public class TrashData_Scene2 : ScriptableObject
{
    public string itemName;

    // แยกออกมาเป็น Dropdown ให้เลือกได้เลยใน Unity
    public TrashType_Scene2 trashType;

    public Sprite icon;
}
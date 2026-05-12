using UnityEngine;

// ประเภทย่อยของขยะ
public enum S2_TrashType
{
    None,
    Glass,   // 0
    Plastic,      // 1
    Metal,    // 2
    Aluminium       // 3
}

[CreateAssetMenu(fileName = "New Item", menuName = "TrainAI/Item Data")]
public class S2_TrashData : ScriptableObject
{
    public string itemName;

    // แยกออกมาเป็น Dropdown ให้เลือกได้เลยใน Unity
    public S2_TrashType trashType;

    public Sprite icon;
}
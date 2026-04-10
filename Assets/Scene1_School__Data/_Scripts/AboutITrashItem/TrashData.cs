using UnityEngine;

// ประเภทขยะ (ใส่ None ไว้กันพลาด)
public enum TrashType { None, Glass, Plastic , Metal , Aluminium}
// ความหายาก
public enum TrashRarity { Common, Rare }

[CreateAssetMenu(fileName = "NewTrashData", menuName = "NSC/Trash Data")]
public partial class TrashData : ScriptableObject
{
    [Header("ข้อมูลพื้นฐานขยะ")]
    public string trashName;      // ชื่อขยะ (เช่น ขวดพลาสติก)
    public TrashType type;        // ประเภท (พลาสติก, โลหะ ฯลฯ)
    public TrashRarity rarity;    // ความหายาก (Common, Rare)

    [Header("การแสดงผล")]
    public Sprite icon;           // รูปโชว์ใน UI
    public GameObject model3D;    // โมเดลที่จะเสกออกมาในฉาก

    [Header("ค่าพลัง/คะแนน")]
    public float scoreValue = 10f; // คะแนนที่จะได้เมื่อเก็บหรือแยกถูก
}
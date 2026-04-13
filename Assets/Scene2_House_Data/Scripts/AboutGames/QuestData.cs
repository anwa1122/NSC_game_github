using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "freeLanceBilly/QuestData")]
public class QuestData : ScriptableObject
{
    public string questName; // ชื่อเควส
    
    [Header("Quest Info")]
    [TextArea(3, 10)]
    public string clientMessage; // ข้อความที่คนจ้างทักมา
    public Sprite questIcon;      // รูปไอคอนประเภทงาน/รูปคนจ้าง
    
    public enum GameType { AI_Training, Circuit, Coding }
    public GameType type;        // ประเภทมินิเกมที่จะเล่น
    
    [Header("Settings")]
    public bool isRare = false;  // เควสหายาก (ขอบรุ้ง)
    public float baseReward;     // เงินรางวัลพื้นฐาน
    
    [Range(1f, 3f)]
    public float difficultyMultiplier = 1f; // ตัวคูณความยาก
}
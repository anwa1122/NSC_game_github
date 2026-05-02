using UnityEngine;
using System.Collections.Generic;

public enum GameType { AiTraining_Trash, Circuit, Programming }

[CreateAssetMenu(fileName = "NewQuest", menuName = "freeLanceBilly/QuestData")]
public class QuestData : ScriptableObject
{
    public string questName; // ชื่อเควส

    [Header("Quest Info")]
    [TextArea(3, 10)]
    public List<string> clientMessages;
    public Sprite questIcon;      // รูปไอคอนประเภทงาน/รูปคนจ้าง
    public Sprite questImage; // รูปร่างเควส


    public GameType type;        // ประเภทมินิเกมที่จะเล่น

    [Header("Settings")]
    public bool isRare = false;  // เควสหายาก (ขอบรุ้ง)
    public float baseReward;     // เงินรางวัลพื้นฐาน

    [Range(1f, 3f)]
    public float difficultyMultiplier = 1f; // ตัวคูณความยาก

}
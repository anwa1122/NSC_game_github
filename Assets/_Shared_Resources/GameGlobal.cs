using UnityEngine;
using System.Collections.Generic;
using TMPro;

public enum DayPhase
    {
        Morning,   // ช่วงเช้า (ต้องไปเรียน)
        Afternoon, // ช่วงบ่าย (เลิกเรียนแล้ว เก็บขยะ/แยกขยะได้)
        Night      // ช่วงกลางคืน (เตรียมตัวนอน)
    }


public class GameGlobal : MonoBehaviour
{
    public static GameGlobal Instance;

    [Header("Day System")]
    public int currentDay = 1;
    public TextMeshProUGUI dayText;

    [Header("Time System")]
    public DayPhase currentPhase = DayPhase.Morning;   //SleepSystem และ StudyScript เป็นตัวเปลี่ยนเวลา
    public TextMeshProUGUI timeText;

    // เปลี่ยนจากตัวแปรสคริปต์เฉพาะทาง เป็น List ของ GameObject
    [Header("Objects to Reset")]
    public List<GameObject> objectsToReset = new List<GameObject>(); 

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        timeText.text = "Time : " + currentPhase;
        dayText.text = "Day : "+ currentDay;
    }

    public void StartNextDay()
    {
        currentDay++;
        Debug.Log("--- เริ่มต้นวันที่ " + currentDay + " ---");
        // วนลูปเช็คทุก Object ใน List ที่เราลากใส่ไว้ใน Inspector
        foreach (GameObject obj in objectsToReset)
        {
            if (obj == null) continue;

            // หาดูว่าใน Object นั้นมีสคริปต์ไหนที่ใช้ IResettable (เช่น TrashSpawner)
            IResettable resettable = obj.GetComponent<IResettable>();

            if (resettable != null)
            {
                resettable.ResetObject(); // สั่งรันฟังก์ชันรีเซ็ตของสคริปต์นั้นๆ
                Debug.Log("Resetting: " + obj.name);
            }
        }
    }

    public void ChangePhase(DayPhase newPhase)
    {
        currentPhase = newPhase;
    }
}
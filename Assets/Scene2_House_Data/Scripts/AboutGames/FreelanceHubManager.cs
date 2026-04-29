using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
public class FreelanceHubManager : MonoBehaviour
{
    [Header("Game Settings")]
    public GameObject freelancePanel;
    public GameObject slotPrefab; //Prefab ที่จะเป็นแม่แบบให้มินิเกมต่างๆ
    public Transform contentParent; //ตำแหน่งที่จะให้เควสไปอยุ่
    public float questBaseReward;
    public List<QuestData> allQuests; //List quest ที่เรามีทั้งหมดภายในเกม


    public static FreelanceHubManager Instance; // ประกาศตัวแปร Static
    private QuestData data;

    void Awake()
    {
        Instance = this; // ตั้งค่าตัวมันเองให้เป็น Instance กลาง
    }

    void Start()
    {
        GenerateQuestList(); //เรียกใช้ฟังชันตอนเริ่มเกมเลย
    }
    public void GenerateQuestList()
    {
        foreach (QuestData data in allQuests) //เรียกแต่ละตัว สร้างตัวแปร data ที่เป็นประเภทสคริปต์ QuestData ที่เอามาจากภายในลิสต์ allQuests
        {
            GameObject newSlot = Instantiate(slotPrefab, contentParent); //สร้าง ตัวแปรประเภท gameObject เพื่อให้มันไปโผล่ภายในเกม โดยใช้ Instatiate โดย ให้ slotPRefab เป็นแม่แบบ, contentParent เป็นตำแหน่งที่จะโคลนแล้วไปเิกด

            QuestItemSlot slotScript = newSlot.GetComponent<QuestItemSlot>(); //สร้างตัวแปร slotScript ประเภท QuestItemSlot ไปเอา component QuestIteSlot ในตัว newSlot mี่ถูกสร้างมา

            bool isRare = Random.Range(0f, 100f) <= 20f; //ให้สุ่มค่า isRare โดยโอกาศ 20% 

            slotScript.Setup(data, isRare);
            //เรียกใช้ฟังชันใน slotScript ที่มี QuestItemSlot เป็น component ทำให้เรียกใช้ Setup ฟังชันได้ แล้วก้ใส่ตัวแปร data กับค่าความจริง isRare ที่สุ่มมา
            //โดยเอา data ไปเพื่อให้กำหนดว่ารูป และ ชื่อ หรือื่นๆมีค่าเป็นไปตาม data ที่เรียงมาในลิสต์e
        }
    }

    public void ExitFreeLanceHub()
    {
        PC_SystemManager.Instance.ExitWindow(freelancePanel);
    }

    public void RemoveQuest(string target)
    {
        foreach (Transform child in contentParent)
        {
            data = child.gameObject.GetComponent<QuestItemSlot>().currentData;
            if (data.questName == target)
            {
                questBaseReward = data.baseReward;
                Destroy(child.gameObject);
            }
        }
    }
}

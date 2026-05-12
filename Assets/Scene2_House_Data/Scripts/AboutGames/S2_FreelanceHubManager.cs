using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
public class S2_FreelanceHubManager : MonoBehaviour
{
    public static S2_FreelanceHubManager Instance; // ประกาศตัวแปร Static

    [Header("Game Settings")]
    public GameObject freelancePanel;
    public GameObject slotPrefab; //Prefab ที่จะเป็นแม่แบบให้มินิเกมต่างๆ
    public Transform contentParent; //ตำแหน่งที่จะให้เควสไปอยุ่
    public List<S2_QuestData> allQuests; //List quest ที่เรามีทั้งหมดภายในเกม
    public float rarity = 20f;
    public bool regenQuest = false;


    public int allQuestCount = 0;
    public bool hideDetailPanel = false;
    private S2_QuestData data;

    void Awake()
    {
        Instance = this; // ตั้งค่าตัวมันเองให้เป็น Instance กลาง
    }

    void Start()
    {
        GenerateQuestList(); //เรียกใช้ฟังชันตอนเริ่มเกมเลย
    }

    void Update()
    {
        if (regenQuest)
        {
            GenerateQuestList();
            regenQuest = false;
        }

        allQuestCount = contentParent.childCount;

        if (allQuestCount <= 0 && !hideDetailPanel)
        {
            hideDetailPanel = true;
            S2_QuestDetailPanel.Instance.ClosePanel();
        }
    }

    public void GenerateQuestList()
    {
        foreach (S2_QuestData data in allQuests) //เรียกแต่ละตัว สร้างตัวแปร data ที่เป็นประเภทสคริปต์ QuestData ที่เอามาจากภายในลิสต์ allQuests
        {
            GameObject newSlot = Instantiate(slotPrefab, contentParent); //สร้าง ตัวแปรประเภท gameObject เพื่อให้มันไปโผล่ภายในเกม โดยใช้ Instatiate โดย ให้ slotPRefab เป็นแม่แบบ, contentParent เป็นตำแหน่งที่จะโคลนแล้วไปเิกด

            S2_QuestItemSlot slotScript = newSlot.GetComponent<S2_QuestItemSlot>(); //สร้างตัวแปร slotScript ประเภท QuestItemSlot ไปเอา component QuestIteSlot ในตัว newSlot mี่ถูกสร้างมา

            bool isRare = Random.Range(0f, 100f) <= rarity; //ให้สุ่มค่า isRare โดยโอกาศ 20% 

            slotScript.Setup(data, isRare);
            //เรียกใช้ฟังชันใน slotScript ที่มี QuestItemSlot เป็น component ทำให้เรียกใช้ Setup ฟังชันได้ แล้วก้ใส่ตัวแปร data กับค่าความจริง isRare ที่สุ่มมา
            //โดยเอา data ไปเพื่อให้กำหนดว่ารูป และ ชื่อ หรือื่นๆมีค่าเป็นไปตาม data ที่เรียงมาในลิสต์e

        }
        hideDetailPanel = false;
    }

    public void ExitFreeLanceHub()
    {
        S2_PC_SystemManager.Instance.ExitWindow(freelancePanel);
    }

    public void GetQuestData(S2_GameType target, out S2_QuestData outData)
    {
        outData = null;

        foreach (Transform child in contentParent)
        {
            data = child.gameObject.GetComponent<S2_QuestItemSlot>().currentData;
            S2_GameType gameType = data.type;
            if (gameType == target)
            {
                outData = child.gameObject.GetComponent<S2_QuestItemSlot>().currentData;
            }
        }
    }
    public void RemoveQuest(S2_GameType target)
    {
        foreach (Transform child in contentParent)
        {
            data = child.gameObject.GetComponent<S2_QuestItemSlot>().currentData;
            S2_GameType gameType = data.type;
            if (gameType == target)
            {
                Destroy(child.gameObject);
                return;
            }
        }
    }
}

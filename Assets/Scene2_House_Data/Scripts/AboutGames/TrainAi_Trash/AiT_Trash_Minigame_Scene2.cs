using System.Collections.Generic;
using UnityEngine;

public class AiT_Trash_Minigame_Scene2 : MonoBehaviour
{
    public static AiT_Trash_Minigame_Scene2 Instance;

    [Header("QuestName")]
    public string questname = "Programming";

    [Header("Ui Settings")]
    public GameObject aiTrainingPanel;


    [Header("Item Database")]
    public List<TrashData_Scene2> allItems; // ลากไฟล์ ScriptableObject ที่สร้างไว้มาใส่ที่นี่ให้หมด
    public GameObject itemPrefab;    // ตัว Prefab ที่มี DraggableItem_Scene2 ติดอยู่
    public RectTransform spawnArea;  // จุดที่จะให้ไอเทมไปเกิด (เช่น UI Panel)


    void Awake()
    {
        if (Instance == null) Instance = this;
    }
    void Start()
    {
        SpawnRandomItems(4);
    }

    public void SpawnRandomItems(int count)
    {
        if (allItems.Count == 0) return; //ถ้าไม่มีไอเทมก้ไม่ต้องเรียกใช้อันอื่น ส่งคืนกลับไป

        for (int i = 0; i < count; i++) //ก้วนลุปตามจำนวนที่กำหนด
        {
            if (allItems.Count == 0) break; //ถ้าประเภทไอเทมไม่มีก้ไม่ต้องทำงานฟังชันนี้

            TrashData_Scene2 randomData = allItems[Random.Range(0, allItems.Count)]; //ให้ randomData มีค่าเป็น ไอเทมที่สุ่มประเภทมา และจะสุ่มแต่ละ item ในลิสต์อีกเพื่อที่จะไปใส่ให้กับ randomData เพื่อที่จะเอาไปสร้างเป็นตัว gameobject

            Vector3 randomPos = GetRandomPosInArea();
            GameObject newItem = Instantiate(itemPrefab, spawnArea);  //สร้าง newItem มาใหม่โดยจะมีแม่แบบเป็น itemPrefab และตำแหน่งเกิดที่ spawnLocation

            newItem.transform.localPosition = GetRandomPosInArea();
            if (newItem.TryGetComponent<DraggableTrash_Scene2>(out var draggable)) //จะไปเอา component ที่เป็นสคริปต์ DraggableItem_Scene2 ให้ออกมาเป็น ตัวแปร draggable
            {
                draggable.SetupItem(randomData); //ในตัว draggable นี้จะมีค่าเป็นตัวสคริปต์ DraggableItem ทำให้เรียกใช้ฟังชั่น Setup เรียกใช้แล้วก้ส่งข้อมูลออกไปเป็น randomData ก้คือ Item ที่เราสุ่มมาทั้งหมดแล้วแล้วให้มันไปแสดงผล ไปมีค่าในตัว Draggable เพื่อที่จะนำไปใช้ในการแยกประเภทต่อ
            }
        }
    }

    public void ExitGame()
    {
        PC_SystemManager.Instance.ExitWindow(aiTrainingPanel);
    }

    private Vector3 GetRandomPosInArea()
    {
        float x = Random.Range(-spawnArea.rect.width / 2.5f, spawnArea.rect.width / 2.5f);
        float y = Random.Range(-spawnArea.rect.height / 2.5f, spawnArea.rect.height / 2.5f);
        return new Vector3(x, y, 0);
    }
}
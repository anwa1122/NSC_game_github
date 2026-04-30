using System.Collections.Generic;
using UnityEngine;

public class TrainAiMinigame : MonoBehaviour
{
    [Header("Ui Settings")]
    public GameObject aiTrainingPanel;


    [Header("Item Database")]
    public List<ItemData_Scene2> allItems; // ลากไฟล์ ScriptableObject ที่สร้างไว้มาใส่ที่นี่ให้หมด
    public GameObject itemPrefab;    // ตัว Prefab ที่มี DraggableItem_Scene2 ติดอยู่
    public Transform spawnLocation;  // จุดที่จะให้ไอเทมไปเกิด (เช่น UI Panel)

    void Start()
    {
        SpawnRandomItems(4);
    }

    public void SpawnRandomItems(int count)
    {
        if (allItems.Count == 0) return; //ถ้าไม่มีไอเทมก้ไม่ต้องเรียกใช้อันอื่น ส่งคืนกลับไป

        ItemCategory_Scene2 selectedCategory = (ItemCategory_Scene2)Random.Range(0, 2); //กำหนดให้ ตัวแปรการเลือกประเภทเป็น การสุ่มของประเภทในสคริปต์ของ ItemCategory_Scene2 ที่มีอยุ่สองประเภท

        List<ItemData_Scene2> filteredItems = allItems.FindAll(item => item.category == selectedCategory); //ก้คือการไปเอาไอเทมที่มีประเภทเดียวกันโดย ฟังชัน allIetms. ตรวจหาลูกทั้งหมด (กำหนดเป็นตัวแปร item แล้วเข้าไปดู ตัวแปร category ของไอเทมนั้นๆว่ามันตรงกับ ประเภทที่ตัวเกมเลือกมั้ย)
        for (int i = 0; i < count; i++) //ก้วนลุปตามจำนวนที่กำหนด
        {
            if (filteredItems.Count == 0) break; //ถ้าประเภทไอเทมไม่มีก้ไม่ต้องทำงานฟังชันนี้

            ItemData_Scene2 randomData = filteredItems[Random.Range(0, filteredItems.Count)]; //ให้ randomData มีค่าเป็น ไอเทมที่สุ่มประเภทมา และจะสุ่มแต่ละ item ในลิสต์อีกเพื่อที่จะไปใส่ให้กับ randomData เพื่อที่จะเอาไปสร้างเป็นตัว gameobject
 
            GameObject newItem = Instantiate(itemPrefab, spawnLocation);  //สร้าง newItem มาใหม่โดยจะมีแม่แบบเป็น itemPrefab และตำแหน่งเกิดที่ spawnLocation

            if (newItem.TryGetComponent<DraggableItem_Scene2>(out var draggable)) //จะไปเอา component ที่เป็นสคริปต์ DraggableItem_Scene2 ให้ออกมาเป็น ตัวแปร draggable
            {
                draggable.SetupItem(randomData); //ในตัว draggable นี้จะมีค่าเป็นตัวสคริปต์ DraggableItem ทำให้เรียกใช้ฟังชั่น Setup เรียกใช้แล้วก้ส่งข้อมูลออกไปเป็น randomData ก้คือ Item ที่เราสุ่มมาทั้งหมดแล้วแล้วให้มันไปแสดงผล ไปมีค่าในตัว Draggable เพื่อที่จะนำไปใช้ในการแยกประเภทต่อ
            }
        }
    }

    public void ExitGame()
    {
        PC_SystemManager.Instance.ExitWindow(aiTrainingPanel);
    }
}
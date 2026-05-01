using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AiT_Trash_Minigame_Scene2 : MonoBehaviour
{
    public static AiT_Trash_Minigame_Scene2 Instance;

    [Header("QuestName")]
    public string questname = "Programming";

    [Header("Ui Settings")]
    public GameObject aiTrainingPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI itemLeftText;
    public Image fillImage;

    [Header("Item Database")]
    public List<TrashData_Scene2> allItems; // ลากไฟล์ ScriptableObject ที่สร้างไว้มาใส่ที่นี่ให้หมด
    public GameObject itemPrefab;    // ตัว Prefab ที่มี DraggableItem_Scene2 ติดอยู่
    public RectTransform spawnArea;  // จุดที่จะให้ไอเทมไปเกิด (เช่น UI Panel)

    [Header("Game Settings")]
    public bool thisGameStart = false;
    public int allItemsInGames = 4;
    public List<Transform> allSlots;
    public float score = 0f;
    public bool thisRare = false;

    private bool gameAlreadyStart = false;
    private bool alreadyChecked = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        scoreText.text = "Score : " + score;
        itemLeftText.enabled = false;
    }

    void ResetGame()
    {
        gameAlreadyStart = false;
        alreadyChecked = false;
    }
    public void StartSmoothLoading(float duration)
    {
        StartCoroutine(SmoothFillRoutine(duration));
    }

    IEnumerator SmoothFillRoutine(float duration)
    {
        float elapsed = 0f;
        float pauseAt = Random.Range(0.3f, 0.7f); // จุดที่จะหยุดสุ่มที่ 30-70%

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            fillImage.fillAmount = progress;

            // ถ้าถึงจุดที่สุ่มไว้ ให้หยุดรอแป๊บนึง (0.2 - 0.5 วิ)
            if (progress >= pauseAt)
            {
                yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
                pauseAt = 2f; // ตั้งค่าให้เกิน 1 เพื่อไม่ให้หยุดซ้ำ
            }
            yield return null;
        }
        fillImage.fillAmount = 1f;
    }
    void Update()
    {
        if (aiTrainingPanel.activeSelf && !thisGameStart && !gameAlreadyStart)
        {
            thisGameStart = true;
            StartSmoothLoading(5f);
            thisRare = QuestDetailPanel.Instance.sendQuestRare;
            if (thisRare)
            {
                rarityText.text = "Rare";
                allItemsInGames = Random.Range(5, 8);
            }
            else
            {
                rarityText.text = "Common";
                allItemsInGames = Random.Range(4, 5);
            }

            SpawnRandomItems(allItemsInGames);
            gameAlreadyStart = true;
        }
        else if (!aiTrainingPanel.activeSelf) thisGameStart = false;
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

    public void CheckItemInSlot()
    {
        if (alreadyChecked) return;



        if (spawnArea.childCount > 0)
        {

            StopAllCoroutines();
            StartCoroutine(ShowItemLeftText());
            return;
        }

        itemLeftText.enabled = false;

        foreach (Transform slot in allSlots)
        {
            foreach (Transform child in slot)
            {
                TrashData_Scene2 itemData = child.GetComponent<DraggableTrash_Scene2>().itemData;
                TrashType_Scene2 trashSlotType = slot.GetComponent<TrashSlot_Scene2>().trashType;
                if (itemData.trashType == trashSlotType)
                {
                    Debug.Log("Correct");
                    score += 10;
                    scoreText.text = "Score : " + score;
                    alreadyChecked = true;
                }
                else
                {
                    Debug.Log("InCorrect");
                }
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

    IEnumerator ShowItemLeftText()
    {
        itemLeftText.text = "There are " + spawnArea.childCount + " items left";
        itemLeftText.enabled = true;

        // รอเป็นเวลา 2 วินาที
        yield return new WaitForSeconds(2f);

        itemLeftText.enabled = false;
    }
}
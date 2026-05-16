using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class S2_AiTManager_Trash : MonoBehaviour
{
    public static S2_AiTManager_Trash Instance;

    [Header("QuestName")]
    public S2_GameType questType;

    [Header("Ui Settings")]
    public GameObject aiTrainingPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI itemLeftText;
    public Image fillImage;
    public TextMeshProUGUI barText;

    [Header("Item Database")]
    public List<S2_TrashData> allItems; // ลากไฟล์ ScriptableObject ที่สร้างไว้มาใส่ที่นี่ให้หมด
    public GameObject itemPrefab;    // ตัว Prefab ที่มี DraggableItem_Scene2 ติดอยู่
    public RectTransform spawnArea;  // จุดที่จะให้ไอเทมไปเกิด (เช่น UI Panel)

    [Header("Game Settings")]
    public int allItemsInGames = 4;
    public List<Transform> allSlots;
    public float score = 0f;
    public float percent = 0f;
    public bool thisRare = false;
    public bool thisGameStart = false;
    public bool playerWinTheGame = false;

    private S2_QuestData thisQuestData = null;

    private bool onTrainingAi = false;
    private bool gameAlreadyStart = false;
    private bool alreadyChecked = false;
    private bool addMoney = false;

    private float allItemsCount;
    private float correctItem;
    private float incorrectItem;

    void Awake()
    {
        if (Instance == null) Instance = this;
        scoreText.text = "Score : " + score;
        itemLeftText.enabled = false;
        barText.enabled = false;
    }

    void ResetGame()
    {
        gameAlreadyStart = false;
        alreadyChecked = false;
        playerWinTheGame = false;
    }

    void Update()
    {
        if (aiTrainingPanel.activeSelf && !thisGameStart && !gameAlreadyStart)
        {
            thisGameStart = true;

            thisRare = S2_QuestDetailPanel.Instance.sendQuestRare;
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

            S2_TrashData randomData = allItems[Random.Range(0, allItems.Count)]; //ให้ randomData มีค่าเป็น ไอเทมที่สุ่มประเภทมา และจะสุ่มแต่ละ item ในลิสต์อีกเพื่อที่จะไปใส่ให้กับ randomData เพื่อที่จะเอาไปสร้างเป็นตัว gameobject

            Vector3 randomPos = GetRandomPosInArea();
            GameObject newItem = Instantiate(itemPrefab, spawnArea);  //สร้าง newItem มาใหม่โดยจะมีแม่แบบเป็น itemPrefab และตำแหน่งเกิดที่ spawnLocation

            newItem.transform.localPosition = GetRandomPosInArea();
            if (newItem.TryGetComponent<S2_DraggableTrash>(out var draggable)) //จะไปเอา component ที่เป็นสคริปต์ DraggableItem_Scene2 ให้ออกมาเป็น ตัวแปร draggable
            {
                draggable.SetupItem(randomData); //ในตัว draggable นี้จะมีค่าเป็นตัวสคริปต์ DraggableItem ทำให้เรียกใช้ฟังชั่น Setup เรียกใช้แล้วก้ส่งข้อมูลออกไปเป็น randomData ก้คือ Item ที่เราสุ่มมาทั้งหมดแล้วแล้วให้มันไปแสดงผล ไปมีค่าในตัว Draggable เพื่อที่จะนำไปใช้ในการแยกประเภทต่อ
            }
        }
    }

    public void CheckItemInSlot()
    {
        if (alreadyChecked)
        {
            S2_NotificationManager.Instance.showNotificationNormally("You have already checked.");
            return;
        }



        if (spawnArea.childCount > 0)
        {

            StopAllCoroutines();
            StartCoroutine(ShowItemLeftText());
            return;
        }

        itemLeftText.enabled = false;
        alreadyChecked = true;

        foreach (Transform slot in allSlots)
        {
            foreach (Transform child in slot)
            {
                S2_TrashData itemData = child.GetComponent<S2_DraggableTrash>().itemData;
                S2_TrashType trashSlotType = slot.GetComponent<S2_TrashSlot>().trashType;
                allItemsCount++;
                if (itemData.trashType == trashSlotType)
                {
                    correctItem++;
                    Debug.Log("Correct");
                    score += 10;
                    scoreText.text = "Score : " + score;


                }
                else
                {
                    incorrectItem++;
                    Debug.Log("InCorrect");
                }
            }
        }

        StartSmoothLoading(5f);
        //มีสคริปต์สรุปผลเปอร์เซ็นถูกต้องด้วย
    }


    public void ExitGame()
    {
        if (!onTrainingAi)
        {
            S2_PC_SystemManager.Instance.ExitWindow(aiTrainingPanel);
            S2_NotificationManager.Instance.stopNotification();
        }
        else
        {
            S2_NotificationManager.Instance.showNotificationNormally("You can't leave right now");
        }
    }

    public void completeAiTraining_TrashGame()
    {
        playerWinTheGame = true;
        //FreelanceHubManager.Instance.RemoveQuest(questType);
        //ไปเอาค่า questBaseReward ด้วย
        S2_FreelanceHubManager.Instance.GetQuestData(questType, out thisQuestData);
        S2_NotificationManager.Instance.showNotificationWithSetTime("Completed the game", 5f);
        if (!addMoney)
        {
            addMoney = true;
            S2_FreelanceHubManager.Instance.RemoveQuest(questType);
            if (S2_PlayerMoneyTest.Instance == null) return;
            S2_PlayerMoneyTest.Instance.AddMoney(score / 10 * thisQuestData.baseReward); ///////////////////////////////////// MONEY PLUS
        }
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

    public void StartSmoothLoading(float duration)
    {
        StartCoroutine(SmoothFillRoutineAndWinTheGame(duration));
    }

    IEnumerator SmoothFillRoutineAndWinTheGame(float duration)
    {
        float elapsed = 0f;
        float pauseAt = Random.Range(0.3f, 0.7f); // จุดที่จะหยุดสุ่มที่ 30-70%
        barText.enabled = true;
        barText.text = "Loading";

        onTrainingAi = true;

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

        onTrainingAi = false;

        fillImage.fillAmount = 1f;
        percent = (correctItem / allItemsCount) * 100;
        barText.text = "Done Accuracy : " + percent + "%";

        completeAiTraining_TrashGame();
    }
}
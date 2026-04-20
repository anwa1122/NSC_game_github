using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using Unity.VisualScripting;
using System.Collections.Generic;
public class QuestDetailPanel : MonoBehaviour
{
    public static QuestDetailPanel Instance; //ถูกเรียกใช้ที่ QuestItemSlot

    [Header("UI Elements")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI descriptionText;

    public Image previewImage;

    [Header("Animation Settings")]
    public float showSpeed = 2f;
    public Vector2 showPosition;
    public Vector2 hidePosition;

    private bool isShow = false;
    private bool doneHide = false;
    
    private RectTransform rect;
    private QuestData currentLoadedData;

    [UnitHeaderInspectable("UI Minigames")]
    public List<GameObject> allMinigames;    
    
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        rect.anchoredPosition = hidePosition;
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Update()
    {
        if (isShow)
        {          
            if (!doneHide)
            {
                rect.anchoredPosition = hidePosition;
                doneHide = true;
            }
        
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, showPosition, Time.deltaTime * showSpeed);

            if (rect.anchoredPosition.x == showPosition.x + 100f)
            {
                isShow = false;
            }
        }
    }

    public void DisplayQuest(QuestData data, bool isRare)
    {
        currentLoadedData = data;

        titleText.text = data.questName;
        rewardText.text = "Reward : " + data.baseReward.ToString() + "$";
        descriptionText.text = data.clientMessages[Random.Range(0, data.clientMessages.Count)];;
        previewImage.sprite = data.questImage;

        if (isRare)
        {
            rarityText.text = "Rarity : rare";
        }
        else  rarityText.text = "Rarity : common";

        isShow = true;
        doneHide = false;
    }

    public void ClosePanel()
    {
        isShow = false;
        rect.anchoredPosition = hidePosition;
    }

    public void OnAcceptQuest()
    {
        if (currentLoadedData != null)
        {
            Debug.Log("เริ่มทำเควส: " + currentLoadedData.questName);
            Debug.Log("ประเภทเกมคือ: " + currentLoadedData.type);

            //isShow = false; // ปิดหน้าต่าง Detail ลงไปก่อนe
            foreach (GameObject minigame in allMinigames)
            {
                if (minigame.name == currentLoadedData.name)
                {
                    minigame.SetActive(true);
                }
            }
        }

    }
}

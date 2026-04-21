using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class ProgrammingMinigame : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI textPrefab; 
    public TMP_InputField playerInput; 
    public GameObject programmingPanel;
    public Transform parent;             

    [Header("Settings")]
    public List<string> wordList;
    public float fallSpeed = 400f;       
    public int totalWordsPerJob = 5;     

    private string currentWord;
    private int wordsCompleted = 0;
    private RandomWord currentWordScript; // เก็บสคริปต์ของคำที่กำลังแสดงอยู่
    private RectTransform currentRect;

    void OnEnable()
    {
        wordsCompleted = 0;
        playerInput.text = "";
        playerInput.ActivateInputField(); 
        RandomizeWord();
    }

    public void RandomizeWord()
    {
        if (wordsCompleted >= totalWordsPerJob)
        {
            WinGame();
            return;
        }

        if (wordList.Count > 0)
        {
            // 1. สุ่มคำ
            currentWord = wordList[Random.Range(0, wordList.Count)];

            // 2. สร้างคำใหม่
            TextMeshProUGUI newWordObj = Instantiate(textPrefab, parent);
            newWordObj.gameObject.SetActive(true);

            // 3. ตั้งค่าตัวลูก
            currentWordScript = newWordObj.GetComponent<RandomWord>();
            if (currentWordScript != null)
            {
                currentWordScript.Setup(currentWord);
            }

            currentRect = newWordObj.GetComponent<RectTransform>();
            playerInput.text = ""; 
            playerInput.ActivateInputField();

            StopAllCoroutines();
            StartCoroutine(FlowDownRoutine());
        }
    }

    IEnumerator FlowDownRoutine()
    {
        if (currentRect == null) yield break;
        currentRect.anchoredPosition = new Vector2(0, 500);

        while (currentRect != null && currentRect.anchoredPosition.y > 0.1f)
        {
            currentRect.anchoredPosition = Vector2.MoveTowards(currentRect.anchoredPosition, Vector2.zero, fallSpeed * Time.deltaTime);
            yield return null;
        }
        if (currentRect != null) currentRect.anchoredPosition = Vector2.zero;
    }

    // ลากใส่ On Value Changed (Dynamic String)
    public void OnInputChanged(string input)
    {
        if (input == currentWord)
        {
            // 1. สั่งตัวลูกที่ถืออยู่ให้ทำลายตัวเองทิ้งทันที
            if (currentWordScript != null)
            {
                currentWordScript.SelfDestruct();
            }

            // 2. นับคะแนนและเริ่มคำใหม่
            wordsCompleted++;
            RandomizeWord();
        }
    }

    void WinGame()
    {
        if(PlayerDataManager.Instance != null) PlayerDataManager.Instance.AddMoney(40f);
        ExitGame();
    }

    public void ExitGame()
    {
        // ล้างคำที่ค้างอยู่ถ้ามี
        if (currentWordScript != null) currentWordScript.SelfDestruct();
        programmingPanel.SetActive(false);
    }
}
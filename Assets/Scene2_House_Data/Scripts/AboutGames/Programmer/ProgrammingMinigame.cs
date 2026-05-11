using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEditor.UI;

public class ProgrammingMinigame : MonoBehaviour
{
    [Header("QuestName")]
    public GameType questType;

    [Header("About Spawning")]
    public RectTransform leftUpperLimit;
    public RectTransform rightBelowLimit;
    public RectTransform spawnParent;
    public int numberOfObjectToSpawn = 5;
    public GameObject errorBlockPrefab;

    [Header("Game Settings")]
    public List<string> allText;
    public TMP_InputField inputField;
    public TextMeshProUGUI scoreText;
    public float scoreRatio = 100f;
    public GameObject programmingPanel;

    [Header("Winning bool")]
    public bool playerWinTheGame = false;

    private QuestData thisQuestData = null;
    private float scorePoint = 0f;
    private bool addOneTime = false;
    private bool lockedWord;
    private string errorWord;
    private ErrorBlock erBlock;
    private ErrorBlock erBlock2;

    void OnEnable()
    {
        OnGameStart();
    }
    public void ResetGame()
    {

    }
    void Update()
    {
        if (inputField.text == null || inputField.text == "")
        {
            lockedWord = false;
            erBlock = null;
            errorWord = "";
            inputField.text = "";

            if (erBlock2 != null) erBlock2.UnLock();
        }
        if (spawnParent.childCount < 1)
        {
            if (!playerWinTheGame) winTheGame();
        }
    }
    public void onValueChange(string word)
    {
        if (string.IsNullOrEmpty(word)) return; // ถ้าช่องว่างไม่ต้องทำอะไร

        string lastChar = word[word.Length - 1].ToString();


        if (!lockedWord)
        {
            foreach (Transform child in spawnParent) //ไปหาลูกๆแต่ละตัว
            {
                TextMeshProUGUI tmpro = child.GetComponentInChildren<TextMeshProUGUI>(); //ไปเอาคำมา
                if (tmpro.text.StartsWith(lastChar))  //ถ้าลูกตัวนั้น คำมันขึ้นต้นด้วย คำหลังสุดที่ Player พิมพ์มา เงื่อนไข = true
                {
                    erBlock = child.GetComponentInChildren<ErrorBlock>(); //ไปบอกโค้ดล๊อค
                    erBlock2 = child.GetComponentInChildren<ErrorBlock>();
                    erBlock.LockAndMoveToTop(); //ให้อยู่บนสุดหน้า ui เพื่อที่มมันจะได้ไม่ซ้อนอันอื่น
                    lockedWord = true;
                    errorWord = tmpro.text;
                    break;
                }
            }
        }

        if (lockedWord)
        {
            if (word == errorWord)
            {
                erBlock.destroyMySelf();
                lockedWord = false;

                correctWord();

                erBlock = null;
                errorWord = "";
                inputField.text = "";
            }
        }
    }
    public void OnGameStart()
    {
        SpawnObject();
        scoreText.text = "Score : " + scorePoint;
    }

    void correctWord()
    {
        scorePoint += 10;
        scoreText.text = "Score : " + scorePoint;
    }

    void winTheGame()
    {
        playerWinTheGame = true;
        //FreelanceHubManager.Instance.RemoveQuest(questType);
        //ไปเอาค่า questBaseReward ด้วย
        FreelanceHubManager.Instance.GetQuestData(questType, out thisQuestData);
        Debug.Log("Got quest data : " + thisQuestData);

        if (!addOneTime)
        {
            addOneTime = true;
            FreelanceHubManager.Instance.RemoveQuest(questType);
            if (PlayerMoneyTest_Scene2.Instance == null) return;
            PlayerMoneyTest_Scene2.Instance.AddMoney(scorePoint / scoreRatio * thisQuestData.baseReward);
        }
    }

    public void ExitGame()
    {
        PC_SystemManager.Instance.ExitWindow(programmingPanel);
    }

    void SpawnObject()
    {
        float maxX = leftUpperLimit.anchoredPosition.x;
        float minX = rightBelowLimit.anchoredPosition.x;

        float maxY = leftUpperLimit.anchoredPosition.y;
        float minY = rightBelowLimit.anchoredPosition.y;

        for (int i = 0; i < numberOfObjectToSpawn; i++)
        {
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);

            Vector2 randomPos = new Vector2(randomX, randomY);

            GameObject newObj = Instantiate(errorBlockPrefab, spawnParent);

            RectTransform rt = newObj.GetComponent<RectTransform>();
            TextMeshProUGUI objt = newObj.GetComponentInChildren<TextMeshProUGUI>();
            if (objt != null)
            {
                int randomIndex = Random.Range(0, allText.Count);

                string selectedText = allText[randomIndex];

                objt.text = selectedText;
            }
            if (rt != null)
            {
                rt.anchoredPosition = randomPos;
            }
        }
    }
}
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;

public class ProgrammingMinigame : MonoBehaviour
{
    [Header("About Spawning")]
    public RectTransform leftUpperLimit;
    public RectTransform rightBelowLimit;
    public RectTransform spawnParent;
    public int numberOfObjectToSpawn = 5;
    public GameObject errorBlockPrefab;

    [Header("Game Settings")]
    public List<string> allText;
    public InputField inputField;


    private bool lockedWord;
    void Start()
    {
        OnGameStart();
    }
    public void onValueChange(string word)
    {
    if (string.IsNullOrEmpty(word)) return; // ถ้าช่องว่างไม่ต้องทำอะไร

    if (!lockedWord)
    {
        string lastChar = word[word.Length - 1].ToString();

            foreach(Transform child in spawnParent)
            {
                TextMeshProUGUI tmpro = child.GetComponentInChildren<TextMeshProUGUI>();
                if (tmpro.text.StartsWith(lastChar)) 
                {
                    ErrorBlock erBlock = child.GetComponentInChildren<ErrorBlock>();
                    erBlock.LockAndMoveToTop();
                    lockedWord = true;
                    Debug.Log(tmpro.text);
                    // เก็บคำนี้ไว้ในตัวแปรอื่นเพื่อเอาไปจัดการต่อ...
                    break; // เจอแล้วหยุดหาตัวอื่นในรอบนี้
                }
            }
        }
    }
    public void OnGameStart()
    {
        SpawnObject();
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
                int randomIndex = Random.Range(0,allText.Count);

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
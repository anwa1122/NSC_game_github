using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ProgrammingMinigame : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI targetText;      // ข้อความเป้าหมายที่สุ่มมา
    public TMP_InputField playerInput;     // ช่องที่ให้ผู้เล่นพิมพ์
    public GameObject programmingPanel;
    
    [Header("Settings")]
    public List<string> wordList;          // รายการคำศัพท์ที่อยากให้สุ่ม
    private string currentWord;            // คำปัจจุบันที่ต้องพิมพ์

    // ฟังก์ชันนี้จะทำงานทุกครั้งที่หน้าต่างมินิเกมถูกเปิด (SetActive(true))
    void OnEnable()
    {
        RandomizeWord();
    }

    public void RandomizeWord()
    {
        if (wordList.Count > 0)
        {
            int randomIndex = Random.Range(0, wordList.Count);
            currentWord = wordList[randomIndex];
            targetText.text = currentWord;
            playerInput.text = ""; // ล้างช่องพิมพ์เดิม
        }
    }

    public void CheckAnswer()
    {
        if (playerInput.text == currentWord)
        {
            Debug.Log("ชนะ! พิมพ์ถูกต้อง");
            PlayerDataManager.Instance.AddMoney(40f);
            // ตรงนี้สามารถเพิ่มระบบให้รางวัล หรือปิดหน้าจอได้
        }
        else
        {
            Debug.Log("ยังไม่ใช่! ลองพิมพ์ใหม่อีกครั้ง");
        }
    }

    public void ExitGame()
    {
        programmingPanel.gameObject.SetActive(false);
        Debug.Log("ออกแล้ว");
    }
}
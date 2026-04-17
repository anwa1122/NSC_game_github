using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // เพิ่มอันนี้มาเพื่อเปลี่ยนฉาก
using System.Collections; // เพิ่มอันนี้มาเพื่อใช้ Coroutine

public class PaySystem_Scene2 : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject payListPanel;
    public TextMeshProUGUI costText;
    public Button payButton;          // ลากปุ่มที่ใช้จ่ายเงินมาใส่
    public CanvasGroup fadePanel;    // สร้าง Image สีดำเต็มจอ แล้วแอด Canvas Group เข้าไป

    [Header("Other settings")]
    public float price;
    public string nextSceneName;     // ชื่อฉากที่จะไป

    public bool passScene2Test = false;

    void OnEnable()
    {
        costText.text = "Price : " + price;
        if(fadePanel != null) fadePanel.alpha = 0; // มั่นใจว่าเริ่มมาจอยังไม่ดำ
    }

    void Update()
    {
        if (passScene2Test) StartCoroutine(FadeAndChangeScene());
    }
    public void playerPayMoney()
    {
        if (PlayerDataManager.Instance.money >= price)
        {
            PlayerDataManager.Instance.money -= price;
            
            // 1. ทำให้ปุ่มกดไม่ได้ทันที
            payButton.interactable = false;

            // 2. เริ่มกระบวนการจอดำและเปลี่ยนฉาก
            StartCoroutine(FadeAndChangeScene());
        }
    }

    IEnumerator FadeAndChangeScene()
    {
        float duration = 2f; // ระยะเวลาที่ต้องการให้จอดำ (วินาที)
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(0, 1, currentTime / duration);
            yield return null;
        }

        // 3. เมื่อจอดำสนิทแล้ว ให้เปลี่ยนฉาก
        SceneManager.LoadScene(nextSceneName);
    }

    public void ExitUi()
    {
        payListPanel.SetActive(false);
    }
}
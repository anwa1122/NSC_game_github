using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinTheGameScript : MonoBehaviour
{
    [Header("Ui and Scene")]
    public CanvasGroup fadePanel;
    public string nextSceneName;

    [Header("Win or not")]
    public bool playerWin = false;
    private bool isChangingScene = false; // เพิ่มตัวแปรเช็คเพื่อไม่ให้รันซ้ำ

    void Start()
    {
        if (fadePanel != null) fadePanel.alpha = 0;
    }

    void Update()
    {
        // เช็คว่าชนะและ "ยังไม่ได้กำลังเปลี่ยนฉาก" ถึงจะรัน
        if (playerWin && !isChangingScene)
        {
            StartCoroutine(FadeAndChangeScene());
        }
    }

    public void PlayerPassScene()
    {
        if (!isChangingScene) StartCoroutine(FadeAndChangeScene());
    }

    public IEnumerator FadeAndChangeScene()
    {
        isChangingScene = true; // ล็อคไว้ว่ากำลังทำงานนะ

        // --- 1. รอ 2 วินาทีก่อนเริ่ม Fade ---


        // --- 2. เริ่มการ Fade จอดำ ---
        float duration = 1f;
        float currentTime = 0;

        if (fadePanel != null)
        {
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                fadePanel.alpha = Mathf.Lerp(0, 1, currentTime / duration);
                yield return null;
            }
        }

        yield return new WaitForSeconds(2f);
        // --- 3. เปลี่ยนฉาก ---
        SceneManager.LoadScene(nextSceneName);
    }
}
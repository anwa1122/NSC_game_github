using UnityEngine;
using UnityEngine.SceneManagement; // เพิ่มอันนี้มาเพื่อเปลี่ยนฉาก
using System.Collections; // เพิ่มอันนี้มาเพื่อใช้ Coroutine
public class WinTheGameScript : MonoBehaviour
{
    [Header("Ui and Scene")]
    public CanvasGroup fadePanel;  
    public string nextSceneName;    

    [Header("Win or not")]
    public bool playerWin = false;

    void Onable()
    {
        if(fadePanel != null) fadePanel.alpha = 0; // มั่นใจว่าเริ่มมาจอยังไม่ดำ 
    }
    void Update()
    {
        if (playerWin) StartCoroutine(FadeAndChangeScene());
    }

    public void PlayerPassScene()
    {
        StartCoroutine(FadeAndChangeScene());
    }

    
    public IEnumerator FadeAndChangeScene()
    {
        float duration = 1f; // ระยะเวลาที่ต้องการให้จอดำ (วินาที)
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
}

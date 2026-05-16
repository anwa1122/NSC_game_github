using System.Collections;
using TMPro;
using UnityEngine;

public class S2_NotificationManager : MonoBehaviour
{
    // ทำให้เป็น Instance เพื่อให้เรียกใช้ได้ง่ายๆ เช่น S2_NotificationManager.Instance.WarnPlayer(...)
    public static S2_NotificationManager Instance { get; private set; }

    [Header("UI References")]
    public TextMeshProUGUI notificationText;
    public CanvasGroup canvasGroup; // แนะนำให้ใส่ CanvasGroup ที่ Text เพื่อคุมความจาง (Alpha)

    [Header("Settings")]
    public float fadeSpeed = 5f;        // ความเร็วในการจางเข้ม
    public float moveSpeed = 5f;        // ความเร็วในการเลื่อน
    public float displayDuration = 2f;  // ระยะเวลาที่แสดงข้อความก่อนจะหายไป
    public Vector3 offsetPos = new Vector3(0, -50, 0); // ตำแหน่งเริ่มต้น (ค่อนลงมาด้านล่าง)

    private Vector3 originalPosition;
    private Coroutine activeCoroutine;

    private void Awake()
    {
        // Setup Singleton
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        // เก็บตำแหน่งเดิมที่จัดไว้ในหน้าจอ
        originalPosition = notificationText.transform.localPosition;

        // ซ่อน Text ไว้ก่อนเริ่มต้น
        if (canvasGroup != null) canvasGroup.alpha = 0;
    }

    public void stopNotification()
    {
        if (activeCoroutine != null) StopCoroutine(activeCoroutine);
        canvasGroup.alpha = 0f;
    }

    // ฟังก์ชันหลักสำหรับเรียกใช้งาน (รับเป็น string จะใช้งานสะดวกกว่า)
    public void showNotificationNormally(string message)
    {
        if (activeCoroutine != null) StopCoroutine(activeCoroutine);
        activeCoroutine = StartCoroutine(AnimateNotification(message));
    }

    public void showNotificationWithSetTime(string message, float time)
    {
        if (activeCoroutine != null) StopCoroutine(activeCoroutine);
        activeCoroutine = StartCoroutine(AnimateNotificationWithSetTime(message, time));
    }

    // Overload เผื่อคุณต้องการส่งผ่าน UI Text ตัวอื่นเข้ามาเหมือนโค้ดเดิม
    public void showNotificationWithYour(UnityEngine.UI.Text textElement)
    {
        showNotificationNormally(textElement.text);
    }

    private IEnumerator AnimateNotification(string message)
    {
        notificationText.text = message;

        // 1. ตั้งค่าเริ่มต้น (เลื่อนลงไปนิดนึง และทำให้โปร่งใส)
        notificationText.transform.localPosition = originalPosition + offsetPos;
        if (canvasGroup != null) canvasGroup.alpha = 0;

        // 2. ช่วง "ค่อยๆ ปรากฏและเลื่อนขึ้น" (Fade & Move In)
        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime * fadeSpeed;

            // เลื่อนตำแหน่งนุ่มๆ
            notificationText.transform.localPosition = Vector3.Lerp(notificationText.transform.localPosition, originalPosition, progress);

            // ปรับความจางนุ่มๆ
            if (canvasGroup != null) canvasGroup.alpha = Mathf.Lerp(0, 1, progress);

            yield return null;
        }

        // 3. รอตามเวลาที่กำหนด
        yield return new WaitForSeconds(displayDuration);

        // 4. ช่วง "ค่อยๆ หายไป" (Fade Out)
        while (canvasGroup != null && canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }

    private IEnumerator AnimateNotificationWithSetTime(string message, float time)
    {
        notificationText.text = message;

        // 1. ตั้งค่าเริ่มต้น (เลื่อนลงไปนิดนึง และทำให้โปร่งใส)
        notificationText.transform.localPosition = originalPosition + offsetPos;
        if (canvasGroup != null) canvasGroup.alpha = 0;

        // 2. ช่วง "ค่อยๆ ปรากฏและเลื่อนขึ้น" (Fade & Move In)
        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime * fadeSpeed;

            // เลื่อนตำแหน่งนุ่มๆ
            notificationText.transform.localPosition = Vector3.Lerp(notificationText.transform.localPosition, originalPosition, progress);

            // ปรับความจางนุ่มๆ
            if (canvasGroup != null) canvasGroup.alpha = Mathf.Lerp(0, 1, progress);

            yield return null;
        }

        // 3. รอตามเวลาที่กำหนด
        yield return new WaitForSeconds(time);

        // 4. ช่วง "ค่อยๆ หายไป" (Fade Out)
        while (canvasGroup != null && canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }
}
using TMPro;
using UnityEngine;
using System.Collections;

public class CantDo_Text : MonoBehaviour,IResettable

{
    public static CantDo_Text Instance;
    public TextMeshProUGUI text;
    public CanvasGroup canvasGroup;
    public float fadeSpeed = 3f;

    public float durationShow = 1f;

    private bool showed = false;

    public void ResetObject()
    {
        showed = false;
        Debug.Log("Reset CantDo_Text");
    }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else                  Destroy(gameObject);
    }

    public void showAlreadyDoneText(string word , bool show)
    {
        if (show)
        {
            if (showed == false)
            {
                text.text = word;
                StopAllCoroutines(); // ป้องกันการรันซ้อนกัน
                StartCoroutine(TimedTextRoutine(word, durationShow));
                showed = true;
            }
            
        }
        else if (!show)
        {
            text.text = "";
            StopAllCoroutines(); // ป้องกันการรันซ้อนกัน
            StartCoroutine(FadeUI(false));
        }
        
    }

    IEnumerator FadeUI(bool fadeIn)
    {
        float targetAlpha = fadeIn ? 1f : 0f; // ถ้า fadeIn เป็น true เป้าหมายคือ 1 (เข้ม)e
        
        if (fadeIn) canvasGroup.gameObject.SetActive(true);

        // วนลูปจนกว่า Alpha จะใกล้เคียงเป้าหมาย
        while (!Mathf.Approximately(canvasGroup.alpha, targetAlpha))
        {
            // ค่อยๆ ปรับ Alpha ไปหาเป้าหมายตามเวลาจริง
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
            yield return null; // รอเฟรมถัดไป
        }
        
        if (!fadeIn) canvasGroup.gameObject.SetActive(false);
    }

    private IEnumerator TimedTextRoutine(string word, float duration)
    {
        text.text = word;
    
        // 1. ค่อยๆ ปรากฏขึ้นมา
        yield return StartCoroutine(FadeUI(true));
    
        // 2. รอตามเวลาที่กำหนด
        yield return new WaitForSeconds(duration);

        showed = false;
        // 3. ค่อยๆ จางหายไป
        yield return StartCoroutine(FadeUI(false));

        
        text.text = ""; // ล้างข้อความเมื่อจบ
    }
}

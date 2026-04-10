using TMPro;
using UnityEngine;
using System.Collections;

public class PlayerHitbox : MonoBehaviour
{
    [Header("CanvasGroup Setting / Ui")]
    public TextMeshProUGUI text;
    public CanvasGroup canvasGroup;
    public float fadeSpeed = 3f;

    [Header("Sitting Scripts")]
    public SitSystem sitSystem;
    public StudyScript studyScript;
    private CollectibleTrash currentTrash; // เก็บค่าขยะที่กำลังแตะอยู่

    private bool foundTrash;
    private bool foundSit;
    private bool foundDoor;
    private bool foundSleep;

    private bool showText;
    private bool foundTrashToCollect;

    private bool doneUiFading;

    void Update()
    {
        // เช็คว่ามีขยะในระยะ และ ผู้เล่นกดปุ่ม E
        if (currentTrash != null && Input.GetKeyDown(KeyCode.E) && foundTrashToCollect)
        {
            CollectTrash();
            StartCoroutine(FadeUI(false)); 
            showText = false;
            foundTrashToCollect = false;
        }

        if (sitSystem.isSitting || studyScript.onStudy) StartCoroutine(FadeUI(false)); 

        if (showText)
        {
            StartCoroutine(FadeUI(true)); 
        }
        else if (!showText && doneUiFading)
        {
            StartCoroutine(FadeUI(false)); 
        }
    }

    private void CollectTrash()
    {
        currentTrash.Collect(); // สั่งให้ขยะทำงาน (เช่น ทำลายตัวเอง/บวกแต้ม)
        currentTrash = null;    // ล้างค่าหลังจากเก็บเสร็จ
    }

    // --- ส่วนการเช็ค Trigger ---
    private void OnTriggerEnter(Collider other)
    {
        foundTrash = other.CompareTag("Trash");
        foundSit = other.CompareTag("SitHitbox");
        foundDoor = other.CompareTag("Door");
        foundSleep = other.CompareTag("SleepHitbox");

        if (foundTrash || foundSit || foundDoor || foundSleep) doneUiFading = true;
    }

    private void OnTriggerStay(Collider other)
    {
        foundTrash = other.CompareTag("Trash");
        foundSit = other.CompareTag("SitHitbox");
        foundDoor = other.CompareTag("Door");
        foundSleep = other.CompareTag("SleepHitbox");

    
        if (foundTrash || foundSit || foundDoor || foundSleep)
        {      
            if (other.TryGetComponent<IDataProvider>(out IDataProvider data))
            {       
                if(data.playerOnUI) showText = false;
                else showText = true;
            }  

             
            if (foundTrash)
            {
                currentTrash = other.GetComponent<CollectibleTrash>();
                text.text = "Press 'E' to collect";
                showText = true;
                foundTrashToCollect = true;
            }
            else if (foundSit)
            {
                text.text = "Press 'E' to sit";
            }
            else if (foundDoor)
            {
                text.text = "Press 'E' to interact";
            }
            else if (foundSleep)
            {
                text.text = "Press 'E' to Sleep";
               
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        foundTrash = other.CompareTag("Trash");
        
        showText = false;
        if (foundTrash)
        {
            currentTrash = null;
            showText = false;
            foundTrashToCollect = false;
        }
    }

    IEnumerator FadeUI(bool fadeIn)
    {
        float targetAlpha = fadeIn ? 1f : 0f; // ถ้า fadeIn เป็น true เป้าหมายคือ 1 (เข้ม)
        
        if (fadeIn) canvasGroup.gameObject.SetActive(true);

        // วนลูปจนกว่า Alpha จะใกล้เคียงเป้าหมาย
        while (!Mathf.Approximately(canvasGroup.alpha, targetAlpha))
        {
            // ค่อยๆ ปรับ Alpha ไปหาเป้าหมายตามเวลาจริง
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
            yield return null; // รอเฟรมถัดไป
        }

        doneUiFading = true;

        if (!fadeIn) canvasGroup.gameObject.SetActive(false);
    }
}
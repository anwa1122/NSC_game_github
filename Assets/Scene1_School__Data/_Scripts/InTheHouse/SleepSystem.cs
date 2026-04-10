using System.Collections; // ต้องมีเพื่อใช้ Coroutine
using UnityEngine;
using UnityEngine.UI;
using System;
using JetBrains.Annotations;

// 1. เพิ่ม , IResettable
public class SleepSystem : MonoBehaviour, IResettable, IDataProvider
{
    [Header("About Sleep")]
    public Transform sleepPoint;
    
    [Header("Scripts")]
    public TrashMiniGameController trashMinigameScript;


    [Header("Ui")]
    public CanvasGroup sleepConfirmCG; // ลาก Panel ที่มี Canvas Group มาใส่
    public float fadeSpeed = 2f;      // ความเร็วในการจาง

    [Header("Black Screen UI")]
    public CanvasGroup blackScreenCG; // ลาก Image สีดำที่มี Canvas Group มาใส่
    public float sleepFadeSpeed = 1f; // ความเร็วตอนหลับ (อาจจะช้ากว่า UI ปกติเพื่อให้ดูนุ่มนวล)

    [Header("Text")]
    public String DefaultText = "You can't sleep rightnow";
    public String morningText = "You can't sleep in the Morning";
    public String haventDoneSortingText = "You haven't done sorting yet";

    private Transform playerTransform;
    private CharacterController playerCharacterController;
    private ThirdPersonCamera cameraState;
    private GameObject playerModel;

    private Vector3 exitPos;
    private Quaternion exitRot;

    private bool canSleep;
    private bool isSleeping = false;

    [HideInInspector]
    public bool onPanel;
    [HideInInspector]
    public bool playerOnUI => onPanel;

    // --- ส่วนที่เพิ่มใหม่: Reset สำหรับวันใหม่ ---
    public void ResetObject()
    {
        canSleep = false;
        isSleeping = false;
        
        // มั่นใจว่าหน้าจอไม่ดำค้างและ UI คำถามถูกปิด
        if (blackScreenCG != null) blackScreenCG.alpha = 0;
        if (sleepConfirmCG != null) 
        {
            sleepConfirmCG.alpha = 0;
            sleepConfirmCG.gameObject.SetActive(false);
        }

        Debug.Log("SleepSystem Reset Done.");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canSleep && !isSleeping)
        {   
            sleepConfirmCG.blocksRaycasts = true;

            StopAllCoroutines(); // ป้องกันการรันซ้อนกัน
            StartCoroutine(FadeUI(true)); 
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            ThirdPersonCamera.Instance.canRotate = false;

            onPanel = true;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSleep = true;
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSleep = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeUI(false)); // สั่ง Fade Out ออกไป

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            canSleep = false;

            ThirdPersonCamera.Instance.canRotate = true;

            if (cameraState != null) cameraState.canRotate = true;
        }
    }

    public void OnClickNo() //กด no แล้วตัวละครลอบขึ้น
    {
        StartCoroutine(FadeUI(false)); // สั่ง Fade Out ออกไป

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ThirdPersonCamera.Instance.canRotate = true;

        onPanel = false;

        if (cameraState != null) cameraState.canRotate = true;
    }

    public void OnClickYes()
    {
        StartCoroutine(FadeUI(false)); 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ThirdPersonCamera.Instance.canRotate = true;

        if (trashMinigameScript.winTheGame && GameGlobal.Instance.currentPhase == DayPhase.Afternoon)
        {       
            isSleeping = true;

            StartCoroutine(SleepSequence());
            CantDo_Text.Instance.showAlreadyDoneText(DefaultText,false);
        }
        else
        {
            if (!trashMinigameScript.winTheGame) DefaultText = haventDoneSortingText;
            if (!(GameGlobal.Instance.currentPhase == DayPhase.Afternoon)) DefaultText = morningText;
            CantDo_Text.Instance.showAlreadyDoneText(DefaultText,true);
        }
        
    }

    IEnumerator FadeUI(bool fadeIn)
    {
        float targetAlpha = fadeIn ? 1f : 0f; // ถ้า fadeIn เป็น true เป้าหมายคือ 1 (เข้ม)
        
        if (fadeIn) sleepConfirmCG.gameObject.SetActive(true);

        // วนลูปจนกว่า Alpha จะใกล้เคียงเป้าหมาย
        while (!Mathf.Approximately(sleepConfirmCG.alpha, targetAlpha))
        {
            // ค่อยๆ ปรับ Alpha ไปหาเป้าหมายตามเวลาจริง
            sleepConfirmCG.alpha = Mathf.MoveTowards(sleepConfirmCG.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
            yield return null; // รอเฟรมถัดไป
        }

        if (!fadeIn) sleepConfirmCG.gameObject.SetActive(false);
    }

    IEnumerator SleepSequence()
{
    // 1. หน้าจอค่อยๆ ดำมืดลง
    float alpha = 0;
    while (alpha < 1)
    {
        alpha += Time.deltaTime * sleepFadeSpeed;
        blackScreenCG.alpha = alpha;
        yield return null;
    }
    blackScreenCG.alpha = 1; // มืดสนิท

    // 2. ช่วงที่จอดำสนิท: ย้ายตำแหน่ง/เปลี่ยนค่าพลังงาน/เปลี่ยนวัน
    Debug.Log("Now Sleeping... Processing Next Day");
    yield return new WaitForSeconds(1.5f); // รอให้ผู้เล่นรู้สึกว่าหลับไปแป๊บนึง

    // 3. หน้าจอค่อยๆ สว่างขึ้น
    while (alpha > 0)
    {
        alpha -= Time.deltaTime * sleepFadeSpeed;
        blackScreenCG.alpha = alpha;
        yield return null;
    }
    blackScreenCG.alpha = 0; // สว่างปกติ

    // 4. คืนอิสระให้ผู้เล่น
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;

    isSleeping = false;

    GameGlobal.Instance.ChangePhase(DayPhase.Morning);
    GameGlobal.Instance.StartNextDay();

    if (cameraState != null) cameraState.canRotate = true;
}
}
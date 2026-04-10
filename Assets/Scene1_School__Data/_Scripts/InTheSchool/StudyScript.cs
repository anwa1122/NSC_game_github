using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Video;

public class StudyScript : MonoBehaviour, IDataProvider
{
    [Header("SittingTransform")]
    public Transform sitTransform;
    [Header("Canvas")]
    public CanvasGroup sittingCanvasGroup;
    public CanvasGroup blackScreenCG;
    public float fadeSpeed;
    public float blackFadeSpeed;
    
    [Header("Text")]
    public String text = "You have already studied";

    private bool canSit = false;
    [HideInInspector]
    public bool onStudy = false;
    private CharacterController playerCharacterController;


    [HideInInspector]
    public bool onPanel;
    [HideInInspector]
    public bool playerOnUI => onPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canSit && !onStudy)
        {
            playerEnterUi();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCharacterController = other.gameObject.GetComponent<CharacterController>();
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSit = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerExitUi();
        }
    }

    public void OnPlayerClickYes()
    {
        if (GameGlobal.Instance.currentPhase == DayPhase.Morning)
        {
            StartCoroutine(FadeUI(false)); 
            playerCharacterController.enabled = false;
            GameGlobal.Instance.ChangePhase(DayPhase.Afternoon);
            StartCoroutine(BlackScreenSequence());
            onStudy = true;
            CantDo_Text.Instance.showAlreadyDoneText(text,false);
        }
        else CantDo_Text.Instance.showAlreadyDoneText(text,true);
        
    }

    public void OnPlayerClickNo()
    {
        playerExitUi();
    }


    private void playerExitUi()
    {
        StartCoroutine(FadeUI(false)); 
        canSit = false;
        onStudy = false;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ThirdPersonCamera.Instance.canRotate = true;

        sittingCanvasGroup.blocksRaycasts = false;

        onPanel = false;
    }

    private void playerEnterUi()
    {
        sittingCanvasGroup.blocksRaycasts = true;

        StopAllCoroutines(); // ป้องกันการรันซ้อนกัน
        StartCoroutine(FadeUI(true)); 
            
        ThirdPersonCamera.Instance.canRotate = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        onPanel = true;
    }

     IEnumerator FadeUI(bool fadeIn)
    {
        float targetAlpha = fadeIn ? 1f : 0f; // ถ้า fadeIn เป็น true เป้าหมายคือ 1 (เข้ม)
        
        if (fadeIn) sittingCanvasGroup.gameObject.SetActive(true);

        while (!Mathf.Approximately(sittingCanvasGroup.alpha, targetAlpha))
        {
            sittingCanvasGroup.alpha = Mathf.MoveTowards(sittingCanvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
            yield return null; // รอเฟรมถัดไป
        }

        if (!fadeIn) sittingCanvasGroup.gameObject.SetActive(false);
    }

        IEnumerator BlackScreenSequence()
    {
        // 1. หน้าจอค่อยๆ ดำมืดลง
        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * blackFadeSpeed;
            blackScreenCG.alpha = alpha;
            yield return null;
        }
        blackScreenCG.alpha = 1; // มืดสนิท

        // 2. ช่วงที่จอดำสนิท: ย้ายตำแหน่ง/เปลี่ยนค่าพลังงาน/เปลี่ยนวัน
        Debug.Log("After a few hours.......");
        yield return new WaitForSeconds(2f); // รอให้ผู้เล่นรู้สึกว่าหลับไปแป๊บนึง

        // 3. หน้าจอค่อยๆ สว่างขึ้น
        while (alpha > 0)
        {
        alpha -= Time.deltaTime * blackFadeSpeed;
        blackScreenCG.alpha = alpha;
        yield return null;
        }
        blackScreenCG.alpha = 0; // สว่างปกติ

        // 4. คืนอิสระให้ผู้เล่น
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        onStudy = false;
        playerCharacterController.enabled = true;
        ThirdPersonCamera.Instance.canRotate = true;

        //GameGlobal.Instance.StartNextDay(); //ติดไว้ก่อนเผื่อจำเป็น
    }
}

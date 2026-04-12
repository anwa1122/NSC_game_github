using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System;

public class DoorSystem : MonoBehaviour, IResettable ,IDataProvider
{
    [Header("CanvasGroup Setting / Ui")]
    public CanvasGroup canvasGroup; 
    public float fadeSpeed;

    [Header("Location to teleport")]
    public Transform teleportPoint;

    [Header("Text")]
    public String text = "Can't go yet need more "+" ";
    private GameObject playerGameObject; 
    private bool canExit;


    [HideInInspector]
    public bool onPanel;
    [HideInInspector]
    public bool playerOnUI => onPanel;

    // 2. ฟังก์ชันรีเซ็ตตามกฎ Interface
    public void ResetObject()
    {    
        // ตรวจสอบ TimeScale เผื่อกรณีมีการหยุดเวลาค้างไว้ตอนข้ามวัน
        Time.timeScale = 1f; 

        Debug.Log("Door System has been reset for the new day.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canExit)
        {
            OpenUI();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerGameObject = other.gameObject; 
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canExit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CloseUI();
        }
    }

    private void OpenUI()
    {
        StopAllCoroutines();
        StartCoroutine(FadeUI(true)); 
            
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        canvasGroup.blocksRaycasts = true;

        CameraState.Instance.ChangeCameraState(CameraMode.FreezeCamera);

        onPanel = true;
    }

    private void CloseUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        CameraState.Instance.ChangeCameraState(CameraMode.UnFreezeCamera);


        StartCoroutine(FadeUI(false)); 

        canExit = false;

        onPanel = false;
    }

    public void OnClickYes()
    {
        // เช็คจำนวนขยะที่ Tag ว่า "Trash" ในฉาก
        int remaining = GameObject.FindGameObjectsWithTag("Trash").Length;

        if (remaining <= 0)
        {
            CharacterController cc = playerGameObject.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false; 
            playerGameObject.transform.position = teleportPoint.position;
            if (cc != null) cc.enabled = true; 
             CantDo_Text.Instance.showAlreadyDoneText(text,false);
            CloseUI();
        }
        else
        {
            text = "Trash remaining : " + remaining + " Trashes You cant go home now";
             CantDo_Text.Instance.showAlreadyDoneText(text,true);
            CloseUI();
        }
    }

    public void OnClickNo()
    {
        CloseUI();
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

        if (!fadeIn) canvasGroup.gameObject.SetActive(false);
    }
}
using UnityEngine;
using System.Collections;
using System.Drawing; // ต้องมีเพื่อใช้ Coroutine
public class TeleportToSchool : MonoBehaviour, IDataProvider
{
    [Header("CanvasGroup Setting / Ui")]
    public CanvasGroup canvasGroup;
    public float fadeSpeed = 1f;

    [Header("Location to teleport")]
    public Transform pointLocation;

    private Transform playerTransform;
    private CharacterController playerCharacterController;

    private bool canExit = false;
    [HideInInspector]
    public bool onPanel;
    [HideInInspector]
    public bool playerOnUI => onPanel;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canExit)
        {   
            playerEnterUi();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.gameObject.GetComponent<Transform>();
            playerCharacterController = other.gameObject.GetComponent<CharacterController>();
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canExit = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
       if (other.CompareTag("Player"))
        {
            playerExitUi();
        } 
    }

    public void playerClickNo()
    {
        playerExitUi();
    }

    public void playerClickYes()
    {
        playerExitUi();

        playerCharacterController.enabled = false;

        playerTransform.transform.position = pointLocation.transform.position;
        playerTransform.transform.rotation = pointLocation.transform.rotation;
        
        playerCharacterController.enabled = true;
    }

    public void playerExitUi()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ThirdPersonCamera.Instance.canRotate = true;

        StartCoroutine(FadeUI(false)); 
        canExit = false;
        onPanel = false;
    }

    private void playerEnterUi()
    {
        canvasGroup.blocksRaycasts = true;

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

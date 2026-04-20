using TMPro;
using UnityEngine;
using System.Collections; // ต้องมีอันนี้เพื่อใช้ IEnumerator

public class SitSystem_Scene2 : MonoBehaviour
{
    [Header("AboutSit")]
    public GameObject E_toSitText;
    public Transform sitPosition;

    private Vector3 exitPos;
    private Quaternion exitRot;
    private Transform playerTransform;
    private CharacterController playerCharacterController;

    private bool canSit;
    private bool playerInUi = false;

    [Header("AboutUI")]
    public GameObject MenuCanvas;
    public float zoomSpeed = 5f; // ความเร็วในการซูม

    void Update()
    {
        if (canSit == true && Input.GetKeyDown(KeyCode.E))
        {
            if (playerTransform == null) return;
            playerPressE();
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.GetComponent<Transform>();
            playerCharacterController = other.GetComponent<CharacterController>();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSit = true;
            if(!playerInUi) E_toSitText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSit = false;
            E_toSitText.SetActive(false);
        }
    }

    public void playerPressE()
    {
        exitPos = playerTransform.position;
        exitRot = playerTransform.rotation;

        playerCharacterController.enabled = false;
        playerTransform.position = sitPosition.position;
        playerTransform.rotation = sitPosition.rotation;
        playerCharacterController.enabled = true;

        MovementState.Instance.ChangeMovementState(MoveMode.StopMoving);
        CameraState.Instance.ChangeCameraState(CameraMode.PCMode);

        EnterUi();
    }

    public void EnterUi()
    {
        E_toSitText.SetActive(false);
        playerInUi = true;
        
        StopAllCoroutines(); 
        StartCoroutine(ZoomInCanvas());
    }

    public void ExitUi()
    {
        playerTransform.position = exitPos;
        playerTransform.rotation = exitRot;

        MovementState.Instance.ChangeMovementState(MoveMode.StartMoving);
        CameraState.Instance.ChangeCameraState(CameraMode.MainCamera);

        playerInUi = false;//e

        StopAllCoroutines(); 
        StartCoroutine(ZoomOutCanvas());
    }
    
    IEnumerator ZoomInCanvas()
    {
        MenuCanvas.SetActive(true);
        MenuCanvas.transform.localScale = Vector3.zero; // เริ่มจาก 0

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * zoomSpeed;
            // ใช้ SmoothStep เพื่อให้การซูมดูนุ่มนวลขึ้น (ช้าช่วงปลาย)
            float smoothT = Mathf.SmoothStep(0, 1, t);
            MenuCanvas.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, smoothT);
            yield return null;
        }
        MenuCanvas.transform.localScale = Vector3.one;
    }

    IEnumerator ZoomOutCanvas()
    {
        MenuCanvas.transform.localScale = Vector3.one; // เริ่มจากขนาดเต็ม 1
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * zoomSpeed;
            // ใช้ SmoothStep เพื่อให้การซูมดูนุ่มนวลขึ้น (ช้าช่วงปลาย)
            float smoothT = Mathf.SmoothStep(0, 1, t);
            MenuCanvas.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, smoothT);
            yield return null;
        }
        MenuCanvas.transform.localScale = Vector3.zero;
        MenuCanvas.SetActive(false);
    }

    
}
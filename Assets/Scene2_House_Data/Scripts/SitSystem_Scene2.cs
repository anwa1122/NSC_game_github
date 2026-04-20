using TMPro;
using UnityEngine;
using System.Collections;

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
    public RectTransform MenuRect; // เปลี่ยนจาก GameObject เป็น RectTransform เพื่อคุมตำแหน่ง UI
    public float animationSpeed = 4f; // ความเร็วรวม
    public float startYOffset = -1000f; // จุดเริ่มต้น (อยู่ใต้จอ)
    public float overshootAmount = 50f; // ระยะที่ให้มันเด้งเลยจุดหมายขึ้นไป

    void Update()
    {
        if (canSit == true && Input.GetKeyDown(KeyCode.E))
        {
            if (playerTransform == null) return;
            if (!playerInUi) playerPressE();
            
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
        StartCoroutine(BounceInCanvas());
    }

    public void ExitUi()
    {
        playerTransform.position = exitPos;
        playerTransform.rotation = exitRot;

        MovementState.Instance.ChangeMovementState(MoveMode.StartMoving);
        CameraState.Instance.ChangeCameraState(CameraMode.MainCamera);

        playerInUi = false;

        StopAllCoroutines(); 
        StartCoroutine(SlideOutCanvas());
    }

    IEnumerator BounceInCanvas()
    {
        MenuRect.gameObject.SetActive(true);
        Vector2 endPos = Vector2.zero; // กลางจอ
        Vector2 startPos = new Vector2(0, startYOffset); // ใต้จอ
        
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * animationSpeed;

            // ใช้สูตร Animation Curve แบบเด้ง (Custom Bounce)
            // สูตรนี้จะเลื่อนขึ้นมา -> เลยจุดหมาย -> แล้วตกลงมาที่จุดเดิม
            float s = t;
            float bounceT = s * s * (3.0f - 2.0f * s); // SmoothStep พื้นฐาน
            
            // เพิ่มการเด้ง (Overshoot)
            float sinBounce = Mathf.Sin(t * Mathf.PI) * overshootAmount;
            
            Vector2 currentPos = Vector2.Lerp(startPos, endPos, bounceT);
            if (t < 1) currentPos.y += sinBounce; // เด้งเฉพาะตอนที่ยังรันอยู่

            MenuRect.anchoredPosition = currentPos;
            yield return null;
        }
        MenuRect.anchoredPosition = endPos;
    }

    IEnumerator SlideOutCanvas()
    {
        Vector2 startPos = MenuRect.anchoredPosition;
        Vector2 endPos = new Vector2(0, startYOffset); // กลับไปใต้จอ
        
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * animationSpeed;
            float smoothT = Mathf.SmoothStep(0, 1, t);
            
            MenuRect.anchoredPosition = Vector2.Lerp(startPos, endPos, smoothT);
            yield return null;
        }
        MenuRect.gameObject.SetActive(false);
    }
}
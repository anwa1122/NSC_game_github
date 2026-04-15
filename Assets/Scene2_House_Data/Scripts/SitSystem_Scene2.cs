using TMPro;
using UnityEngine;

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
        
        playerTransform.transform.position = sitPosition.transform.position;
        playerTransform.transform.rotation = sitPosition.transform.rotation;

        playerCharacterController.enabled = true;

        MovementState.Instance.ChangeMovementState(MoveMode.StopMoving);
        CameraState.Instance.ChangeCameraState(CameraMode.PCMode);

        EnterUi();
    }
    

    public void EnterUi()
    {
        MenuCanvas.SetActive(true);
        E_toSitText.SetActive(false);
        playerInUi = true;
    }

    public void ExitUi()
    {
        MenuCanvas.SetActive(false);

        playerTransform.position = exitPos;
        playerTransform.rotation = exitRot;

        MovementState.Instance.ChangeMovementState(MoveMode.StartMoving);
        CameraState.Instance.ChangeCameraState(CameraMode.MainCamera);

        playerInUi = false;
    }
}

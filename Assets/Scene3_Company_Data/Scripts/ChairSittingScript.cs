using System.Runtime.CompilerServices;
using UnityEngine;

public class ChairSittingScript : MonoBehaviour
{

    public GameObject leaveBtn;
    public Transform sitTransform;


    private Transform playerTransform;
    private CharacterController playerCharacterController;
    private MovementState playerMovementState;
    private CameraState playerCameraState;


    private Vector3 exitPos;
    private Quaternion exitRot;


    private bool canSit;
    private bool isSitting;

    void Start()
    {
        leaveBtn.SetActive(false);
    }
    void Update()
    {
        if (canSit && Input.GetKeyDown(KeyCode.E))
        {
            isSitting = true;
            playerSitState();
        }

        if (ClickManager.Instance.playerClick)
        {
            leaveBtn.SetActive(false);
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCharacterController = other.GetComponent<CharacterController>();
            playerMovementState = other.GetComponent<MovementState>();
            playerCameraState = other.GetComponent<CameraState>();

            playerTransform = other.GetComponent<Transform>();
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
            canSit = false;
        }
    }

    void playerSitState()
    {
        exitPos = playerTransform.position;
        exitRot = playerTransform.rotation;

        playerCharacterController.enabled = false;

        playerCameraState.ChangeCameraState(CameraMode.PCMode);

        playerTransform.position = sitTransform.position;
        playerTransform.rotation = sitTransform.rotation;

        playerMovementState.ChangeMovementState(MoveMode.StopMoving);
        playerCharacterController.enabled = true;

        leaveBtn.SetActive(true);
    }

    public void playerLeave()
    {
        playerCharacterController.enabled = true;

        leaveBtn.SetActive(false);

        playerTransform.position = exitPos;
        playerTransform.rotation = exitRot;

        playerCameraState.ChangeCameraState(CameraMode.MainCamera);
        playerMovementState.ChangeMovementState(MoveMode.StartMoving);
    }
}

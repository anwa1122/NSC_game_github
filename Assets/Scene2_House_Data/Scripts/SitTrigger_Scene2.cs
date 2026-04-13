using UnityEngine;

public class SitTrigger_Scene2 : MonoBehaviour
{
    public Transform sitPosition;
    private Transform playerTransform;
    private CharacterController playerCharacterController;
    private bool canSit;
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
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSit = false;
        }
    }

    void playerPressE()
    {
        playerCharacterController.enabled = false;
        
        playerTransform.transform.position = sitPosition.transform.position;
        playerTransform.transform.rotation = sitPosition.transform.rotation;

        playerCharacterController.enabled = true;

        MovementState.Instance.ChangeMovementState(MoveMode.StopMoving);
        CameraState.Instance.ChangeCameraState(CameraMode.PCMode);
    }
}

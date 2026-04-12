using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SitSystem : MonoBehaviour, IDataProvider
{
    [Header("Other")]
    public EntryPanelManager enterGameSystem;
    public Transform sitLocation;
    private bool playerEPressed;
    private GameObject playerModel;
    private Transform playerTransform;
    private CameraState cameraState;
    private MovementState movementState;

    private Vector3 exitPos;
    private Quaternion exitRot;

    private bool canSit;
    [HideInInspector]
    public bool isSitting;

    [HideInInspector]
    public bool onPanel;
    [HideInInspector]
    public bool playerOnUI => onPanel;
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canSit == true)
        {
            if (playerTransform == null) return;
            if (isSitting == false) enterGame();
            //else exitGame(); เผื่ออยากทำกด e แล้วออกด้วย
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (playerModel == null) playerModel = other.transform.Find("Billyidle").gameObject;
            if (playerTransform == null) playerTransform = other.GetComponent<Transform>();
            if (cameraState == null) cameraState = other.GetComponent<CameraState>();
            if (movementState == null) movementState = other.GetComponent<MovementState>();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canSit = true;

            if (isSitting == true)
            {
                playerModel.SetActive(false);
            }
            else
            {
                playerModel.SetActive(true);
               
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {     
        if (other.gameObject.CompareTag("Player"))
        {
            canSit = false;
            isSitting = false;
            playerTransform = null;
        }
    }

    public void enterGame()
    {
        exitPos = playerTransform.position;
        exitRot = playerTransform.rotation;

        isSitting = true;

        enterGameSystem.SetSittingState(true);

        playerTransform.transform.position = sitLocation.transform.position;
        playerTransform.transform.rotation = sitLocation.transform.rotation;

        cameraState.ChangeCameraState(CameraMode.POVCamera);
        movementState.ChangeMovementState(MoveMode.StopMoving);

        onPanel = true;
    }

    public void exitGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        canSit = true;
        isSitting = false;

        enterGameSystem.SetSittingState(false);

        playerTransform.position = exitPos;
        playerTransform.rotation = exitRot;

        cameraState.ChangeCameraState(CameraMode.MainCamera);
        movementState.ChangeMovementState(MoveMode.StartMoving);

        onPanel = false;
    }
}

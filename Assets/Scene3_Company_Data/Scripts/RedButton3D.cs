using Unity.VisualScripting;
using UnityEngine;

public class Red3DScript : MonoBehaviour
{
    public GameObject minigameInterface;
    public Transform targetPosition;

    public bool playerClicked;
    void Update()
    {
        if (this.gameObject == ClickManager.Instance.clickedObject && ClickManager.Instance.playerClick)
        {
            CameraState.Instance.ChangeCameraPosition(targetPosition);
            UIController_Scene3.Instance.LaunchInterface(minigameInterface);

            ClickManager.Instance.playerClick = false;
        }

    }
    public void playerExit()
    {
        UIController_Scene3.Instance.ExitInterface();
        CameraState.Instance.ChangeCameraPosition(null);
    }
}

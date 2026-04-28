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

        }

    }
    public void playerExit()
    {
        UIController_Scene3.Instance.ExitInterface(minigameInterface);
        CameraState.Instance.ChangeCameraPosition(null);
    }
}

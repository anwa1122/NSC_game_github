using UnityEngine;

public class Blue3DScript : MonoBehaviour
{
    public GameObject minigameInterface;
    public Transform targetPosition;

    void Update()
    {
        if (this.gameObject == ClickManager.Instance.clickedObject)
        {

            CameraState.Instance.ChangeCameraPosition(targetPosition);
            if (ClickManager.Instance.playerClick) UIController_Scene3.Instance.LaunchInterface(minigameInterface);
        }
    }

    public void playerExit()
    {
        UIController_Scene3.Instance.ExitInterface(minigameInterface);
        CameraState.Instance.ChangeCameraPosition(null);
    }
}

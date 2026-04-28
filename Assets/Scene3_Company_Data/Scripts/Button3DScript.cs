using UnityEngine;

public class Button3DScript : MonoBehaviour
{
    public GameObject minigameInterface;
    public Transform targetPosition;
    void Update()
    {
        if (this.gameObject == ClickManager.Instance.clickedObject)
        {
            UIController_Scene3.Instance.LaunchInterface(minigameInterface);
            CameraState.Instance.ChangeCameraPosition(targetPosition);
        }
    }

    public void playerExit()
    {
        UIController_Scene3.Instance.ExitInterface(minigameInterface);
        CameraState.Instance.ChangeCameraPosition(null);
    }
}

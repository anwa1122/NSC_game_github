using UnityEngine;

public class Button3DScript : MonoBehaviour
{
    public GameObject panel;
    public Transform targetPosition;
    void Update()
    {
        if (this.gameObject == ClickManager.Instance.clickedObject)
        {
            panel.SetActive(true);
            CameraState.Instance.ChangeCameraPosition(targetPosition);
        }
    }

    public void playerExit()
    {
        panel.SetActive(false);
        CameraState.Instance.ChangeCameraPosition(null);
    }
}

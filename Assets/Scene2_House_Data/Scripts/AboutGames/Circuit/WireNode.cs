using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Unity.VisualScripting;
// ต้องมี Component Image และเปิด Raycast Target ด้วยนะ
public class WireNode : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public NodeType nodeType;
    private WireManager manager;

    void Start()
    {
        // หา Manager ใน Scene
        manager = Object.FindFirstObjectByType<WireManager>();

        if (manager == null)
            Debug.LogError("เฮ้ย! ลืมวาง WireManager ไว้ใน Scene หรือเปล่า?");

    }

    public void OnPointerDown(PointerEventData eventData)
    {

        // ส่งตัวเอง (RectTransform) ไปให้ Manager เริ่มวาด
        manager.BeginDragWire(GetComponent<RectTransform>(), eventData.position);

        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            DeviceData deviceData = result.gameObject.GetComponent<DeviceData>();
            //WireNode wireNode = result.gameObject.GetComponent<WireNode>();

            if (deviceData != null)
            {
                WireNode redNode = deviceData.redNode1;
                WireNode blackNode = deviceData.blackNode1;

                if (redNode != null && blackNode != null)
                {
                    if (redNode.nodeType == NodeType.Red || blackNode.nodeType == NodeType.Black)
                    {
                        CircuitManager.Instance.addDevice(deviceData.deviceType);
                    }
                }

            }
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        // ส่งตำแหน่งเมาส์ไปให้ Manager อัปเดตเส้น
        manager.UpdateWirePosition(eventData.position);


    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // แจ้ง Manager ว่าปล่อยเมาส์แล้ว

        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            DeviceData deviceData = result.gameObject.GetComponent<DeviceData>();
            //WireNode wireNode = result.gameObject.GetComponent<WireNode>();

            if (deviceData != null)
            {
                WireNode redNode = deviceData.redNode1;
                WireNode blackNode = deviceData.blackNode1;

                if (redNode != null && blackNode != null)
                {
                    if (redNode.nodeType == NodeType.Red || blackNode.nodeType == NodeType.Black)
                    {
                        //Debug.Log("เจอ wireNode");
                        manager.EndDragWire(true);
                        CircuitManager.Instance.addDevice(deviceData.deviceType);
                    }
                }
                else
                {
                    //Debug.Log("ไม่เจอ wireNode");
                    manager.EndDragWire(false);
                }
            }
        }
    }
}
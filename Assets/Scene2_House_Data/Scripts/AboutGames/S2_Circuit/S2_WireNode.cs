using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class S2_WireNode : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Node Type")]
    public S2_NodeType nodeType;
    public S2_NodeClass nodeClass;

    private S2_WireManager wireManager;

    void Start()
    {
        // หา Manager ใน Scene
        wireManager = Object.FindFirstObjectByType<S2_WireManager>();

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (nodeClass == S2_NodeClass.Get) return;
        // เริ่มลากเส้นจากโหนดนี้
        wireManager.BeginDragWire(GetComponent<RectTransform>(), eventData.position);


        wireManager.currentWire.GetComponent<S2_ConnectionInfo>().firstDevice = this.transform.parent.gameObject.GetComponent<S2_DeviceData>().deviceType;
        wireManager.currentWire.GetComponent<S2_ConnectionInfo>().nodeType = nodeType;
        //Debug.Log(this.transform.parent.gameObject.GetComponent<DeviceData>().deviceType);

        S2_DeviceData device = transform.parent.GetComponent<S2_DeviceData>();
        //CircuitManager.Instance.addDevice(device.deviceType);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (nodeClass == S2_NodeClass.Get) return;
        // อัปเดตตำแหน่งเส้นตามเมาส์ขณะลาก
        wireManager.UpdateWirePosition(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // --- ส่วนเช็คที่โหนดอย่างเดียว ---
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        S2_WireNode targetNode = null;

        foreach (RaycastResult result in results)
        {
            // เช็คว่าสิ่งที่เจอมีสคริปต์ WireNode หรือไม่
            S2_WireNode node = result.gameObject.GetComponent<S2_WireNode>();

            // เงื่อนไข: ต้องเป็น WireNode และ "ไม่ใช่ตัวมันเอง"
            if (node != null && node != this && node.nodeClass != this.nodeClass)
            {
                targetNode = node;
                // เข้าถึง GameObject ของพ่อ   /////เฮ้ targetNode! ช่วยไปดูที่ Transform ของ GameObject ที่เธอแปะอยู่ให้หน่อยสิ! //มองย้อนกลับไปหา "พ่อ" // ไปเอาพ่อมันมา
                GameObject parentObj = targetNode.transform.parent.gameObject;
                S2_DeviceData device = parentObj.GetComponent<S2_DeviceData>();

                wireManager.currentWire.GetComponent<S2_ConnectionInfo>().secondDevice = device.deviceType;
                //Debug.Log(device.deviceType);

                S2_CircuitManager.Instance.RefreshRemainDevice();
                break; // เจอโหนดเป้าหมายแล้ว หยุดหาทันที
            }
        }

        if (targetNode != null)
        {
            // ถ้าเจอโหนดปลายทาง ส่งค่า true และส่ง RectTransform ของโหนดนั้นไปให้ Manager ล็อค
            wireManager.EndDragWire(true, targetNode.GetComponent<RectTransform>());
            //Debug.Log("ปล่อยเมาส์เจอโหนดปลายทาง");
        }
        else
        {
            // ถ้าไม่เจออะไรเลย ส่งค่า false เพื่อลบเส้น
            wireManager.EndDragWire(false);
            //Debug.Log("ไม่เจอโหนดปลายทาง ลบเส้นทิ้ง");
        }
    }
}
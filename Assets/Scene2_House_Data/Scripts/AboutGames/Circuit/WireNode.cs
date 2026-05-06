using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class WireNode : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Node Type")]
    public NodeType nodeType;
    public NodeClass nodeClass;

    private WireManager wireManager;

    void Start()
    {
        // หา Manager ใน Scene
        wireManager = Object.FindFirstObjectByType<WireManager>();

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (nodeClass == NodeClass.Get) return;
        // เริ่มลากเส้นจากโหนดนี้
        wireManager.BeginDragWire(GetComponent<RectTransform>(), eventData.position);


        wireManager.currentWire.GetComponent<ConnectionInfo>().firstDevice = this.transform.parent.gameObject.GetComponent<DeviceData>().deviceType;
        //Debug.Log(this.transform.parent.gameObject.GetComponent<DeviceData>().deviceType);

        DeviceData device = transform.parent.GetComponent<DeviceData>();
        CircuitManager.Instance.addDevice(device.deviceType);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (nodeClass == NodeClass.Get) return;
        // อัปเดตตำแหน่งเส้นตามเมาส์ขณะลาก
        wireManager.UpdateWirePosition(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // --- ส่วนเช็คที่โหนดอย่างเดียว ---
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        WireNode targetNode = null;

        foreach (RaycastResult result in results)
        {
            // เช็คว่าสิ่งที่เจอมีสคริปต์ WireNode หรือไม่
            WireNode node = result.gameObject.GetComponent<WireNode>();

            // เงื่อนไข: ต้องเป็น WireNode และ "ไม่ใช่ตัวมันเอง"
            if (node != null && node != this && node.nodeClass != this.nodeClass)
            {
                targetNode = node;
                // เข้าถึง GameObject ของพ่อ   /////เฮ้ targetNode! ช่วยไปดูที่ Transform ของ GameObject ที่เธอแปะอยู่ให้หน่อยสิ! //มองย้อนกลับไปหา "พ่อ" // ไปเอาพ่อมันมา
                GameObject parentObj = targetNode.transform.parent.gameObject;
                DeviceData device = parentObj.GetComponent<DeviceData>();

                wireManager.currentWire.GetComponent<ConnectionInfo>().secondDevice = device.deviceType;
                //Debug.Log(device.deviceType);

                CircuitManager.Instance.addDevice(device.deviceType);
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
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class WireNode : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public NodeType nodeType;
    private WireManager manager;

    void Start()
    {
        // หา Manager ใน Scene
        manager = Object.FindFirstObjectByType<WireManager>();

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // เริ่มลากเส้นจากโหนดนี้
        manager.BeginDragWire(GetComponent<RectTransform>(), eventData.position);
        DeviceData device = transform.parent.GetComponent<DeviceData>();
        CircuitManager.Instance.addDevice(device.deviceType);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // อัปเดตตำแหน่งเส้นตามเมาส์ขณะลาก
        manager.UpdateWirePosition(eventData.position);
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
            if (node != null && node != this)
            {
                targetNode = node;
                // เข้าถึง GameObject ของพ่อ   /////เฮ้ targetNode! ช่วยไปดูที่ Transform ของ GameObject ที่เธอแปะอยู่ให้หน่อยสิ! //มองย้อนกลับไปหา "พ่อ" // ไปเอาพ่อมันมา
                GameObject parentObj = targetNode.transform.parent.gameObject;
                //Debug.Log("ชื่อของพ่อคือ: " + parentObj.name);
                DeviceData device = parentObj.GetComponent<DeviceData>();

                Debug.Log(device.deviceType);
                CircuitManager.Instance.addDevice(device.deviceType);
                break; // เจอโหนดเป้าหมายแล้ว หยุดหาทันที
            }
        }

        if (targetNode != null)
        {
            // ถ้าเจอโหนดปลายทาง ส่งค่า true และส่ง RectTransform ของโหนดนั้นไปให้ Manager ล็อค
            manager.EndDragWire(true, targetNode.GetComponent<RectTransform>());
            Debug.Log("ปล่อยเมาส์เจอโหนดปลายทาง");
        }
        else
        {
            // ถ้าไม่เจออะไรเลย ส่งค่า false เพื่อลบเส้น
            manager.EndDragWire(false);
            Debug.Log("ไม่เจอโหนดปลายทาง ลบเส้นทิ้ง");
        }
    }
}
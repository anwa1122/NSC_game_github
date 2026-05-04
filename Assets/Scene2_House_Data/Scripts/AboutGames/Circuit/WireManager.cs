using UnityEngine;
using UnityEngine.UI;

public class WireManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject wirePrefab;
    public Canvas mainCanvas;

    private GameObject currentWire;
    private RectTransform wireRect;
    private RectTransform activeStartNode;

    // ฟังก์ชันเริ่มสร้างสายไฟ (ถูกเรียกจาก WireNode)
    public void BeginDragWire(RectTransform node, Vector2 screenPos)
    {
        if (wirePrefab == null || mainCanvas == null) return;

        activeStartNode = node;

        // สร้างสายไฟไว้ภายใต้ Parent เดียวกับ Node เพื่อให้ Layer ถูกต้อง
        currentWire = Instantiate(wirePrefab, node.parent);
        wireRect = currentWire.GetComponent<RectTransform>();

        // --- เพิ่มตรงนี้ครับ ---
        // 1. ดึงข้อมูลสีจาก Node ที่เราคลิก (ใช้ NodeInfo ที่เราทำไว้ก่อนหน้า)
        WireNode info = node.GetComponent<WireNode>();
        Image wireImage = currentWire.GetComponent<Image>();

        if (info != null && wireImage != null)
        {
            // 2. เปลี่ยนสี Image ของสายไฟให้ตรงกับประเภท Node
            if (info.nodeType == NodeType.Red) wireImage.color = Color.red;
            else if (info.nodeType == NodeType.Black) wireImage.color = Color.black;
            else wireImage.color = Color.white; // สี default
        }
        // -----------------------

        wireRect.pivot = new Vector2(0f, 0.5f);
        wireRect.position = node.position;
        wireRect.sizeDelta = new Vector2(0, wireRect.sizeDelta.y);

        UpdateWirePosition(screenPos);
    }

    // ฟังก์ชันอัปเดตเส้น (ถูกเรียกจาก WireNode)
    public void UpdateWirePosition(Vector2 screenPos)
    {
        if (wireRect == null || activeStartNode == null) return;

        Vector3 worldMousePos;
        // แปลงพิกัดเมาส์ให้เป็น World Space ของ UI
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            wireRect,
            screenPos,
            mainCanvas.worldCamera,
            out worldMousePos
        );

        Vector3 direction = worldMousePos - activeStartNode.position;

        // 1. หมุนสายไฟ
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        wireRect.rotation = Quaternion.Euler(0, 0, angle);

        // 2. คำนวณความยาว (หารด้วย ScaleFactor เพื่อความแม่นยำของ UI)
        float distance = direction.magnitude / mainCanvas.scaleFactor;

        // ชดเชยสเกลกรณีมีการ Scale จอ
        float finalLength = distance * (1 / wireRect.lossyScale.x);
        wireRect.sizeDelta = new Vector2(finalLength, wireRect.sizeDelta.y);
    }

    public void EndDragWire(bool findNode)
    {
        if (findNode)
        {
        }
        else
        {
            Destroy(currentWire);
        }

        currentWire = null;
        wireRect = null;
    }
}
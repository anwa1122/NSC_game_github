using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DraggableTrash_Scene2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public TrashData_Scene2 itemData;
    private Image displayImage;

    private Vector3 startPosition;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    // อ้างอิงถึงพื้นที่เริ่มต้น (ควรลากมาใส่ใน Inspector หรือหาอัตโนมัติ)
    public RectTransform beginSlotRect;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        // ถ้าไม่ได้ลากมาใส่ ให้ถือว่า Parent ตอนเริ่มคือพื้นที่ BeginSlot
        if (beginSlotRect == null)
            beginSlotRect = transform.parent.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling();

        // จำตำแหน่งก่อนลากไว้เผื่อกรณีฉุกเฉิน
        startPosition = rectTransform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ลากได้อิสระทั่วจอตามมือ
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        GameObject targetSlot = GetMouseUnderSlot();

        if (targetSlot != null)
        {
            // 1. ถ้าปล่อยลง Slot (ที่ไม่ใช่จุดเริ่ม) ให้ย้ายไปตรงนั้น
            if (targetSlot.CompareTag("Slot"))
            {
                rectTransform.position = Input.mousePosition;
                // ถ้าอยากให้เปลี่ยน Parent ไปด้วย
                transform.SetParent(targetSlot.transform);
            }
            // 2. ถ้าปล่อยลง BeginSlot เดิม ให้วางได้ปกติ
            else if (targetSlot.CompareTag("BeginSlot"))
            {
                rectTransform.position = Input.mousePosition;
                transform.SetParent(targetSlot.transform);
            }
        }
        else
        {
            // [โจทย์ของคุณ] ถ้าปล่อยข้างนอก (ไม่เจอ Tag) ให้เด้งไปที่ "ขอบ" ของ BeginSlot ที่ใกล้ที่สุด
            rectTransform.position = GetClosestPointOnRect(beginSlotRect, Input.mousePosition);
        }
    }

    // ฟังก์ชันคำนวณหาจุดที่ใกล้ที่สุดบนขอบของ UI Area
    private Vector3 GetClosestPointOnRect(RectTransform rect, Vector3 mousePos)
    {                                       //ตำแหน่ง beginSlot  //ตำแหน่งเม้าส์
        Vector2 localPoint;
        // แปลงตำแหน่งเมาส์เป็นตำแหน่ง Local ของช่อง BeginSlot
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, mousePos, null, out localPoint);

        // หาขอบเขตของช่อง (ลบขนาดตัวไอเทมออกครึ่งหนึ่งเพื่อให้ไม่ล้นขอบ)
        float halfW = rectTransform.rect.width / 2;
        float halfH = rectTransform.rect.height / 2;

        float minX = rect.rect.xMin + halfW;
        float maxX = rect.rect.xMax - halfW;
        float minY = rect.rect.yMin + halfH;
        float maxY = rect.rect.yMax - halfH;

        // Clamp ตำแหน่งให้อยู่แค่ในขอบ
        localPoint.x = Mathf.Clamp(localPoint.x, minX, maxX);
        localPoint.y = Mathf.Clamp(localPoint.y, minY, maxY);

        // แปลงกลับเป็น World Position เพื่อเอาไปใช้งานกับ transform.position
        return rect.TransformPoint(localPoint);
    }

    private GameObject GetMouseUnderSlot()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Slot") || result.gameObject.CompareTag("BeginSlot"))
            {
                return result.gameObject;
            }
        }
        return null;
    }

    public void SetupItem(TrashData_Scene2 data)
    {
        displayImage = GetComponent<Image>();
        itemData = data;
        if (itemData.icon != null) displayImage.sprite = itemData.icon;
    }
}
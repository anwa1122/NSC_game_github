using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class DraggableTrash_Scene2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public TrashData_Scene2 itemData;
    private Image displayImage;

    public Vector3 startPosition; //ตำแหน่งเริ่มต้
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        startPosition = rectTransform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        canvasGroup.alpha = 0.6f;  //ทำให้มันล่องหน
        canvasGroup.blocksRaycasts = false; // ให้ยิงเลเซอร์ทะลุได้เอาไว้เช็คค่าต่างๆ
        transform.SetAsLastSibling(); // สั่งให้มันเป็นลูกตัวสุดท้ายอง parent นั้นๆเพื่อให้มันอยุ่ด้านบนสุดตลอดในภาพ ui


    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
        KeepInsideScreen();
        GameObject currentObj = GetMouseUnderSlot();
        Debug.Log(currentObj);
        if (currentObj != null) startPosition = Input.mousePosition; ;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // เรียกใช้ฟังก์ชันเช็คที่เราสร้างไว้
        GameObject targetSlot = GetMouseUnderSlot();

        if (targetSlot != null)
        {
            // กรณีเจอ Slot: ให้ไปอยู่ที่ตำแหน่ง Slot นั้น (หรือตำแหน่งเมาส์ตามที่คุณต้องการ)
            rectTransform.position = targetSlot.transform.position;

            // ถ้าอยากให้อยู่ตรงที่ปล่อยเป๊ะๆ ก็ใช้:
            // rectTransform.position = Input.mousePosition;
        }
        else
        {
            // กรณีไม่เจอ Slot เลย: ให้กลับไปที่จุดเริ่มต้น
            rectTransform.position = startPosition;
        }
    }

    private GameObject GetMouseUnderSlot()
    {
        // สร้าง PointerEventData ใหม่ที่ตำแหน่งเมาส์ปัจจุบัน
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        // สร้าง List มารองรับผลลัพธ์
        List<RaycastResult> results = new List<RaycastResult>();

        // สั่งยิง Raycast ทะลุ UI ทั้งหมดที่ตำแหน่งเมาส์
        EventSystem.current.RaycastAll(eventData, results);

        // วนลูปหาเฉพาะอันที่มี Tag ที่เราต้องการ
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Slot") || result.gameObject.CompareTag("BeginSlot"))
            {
                return result.gameObject; // เจอแล้วส่ง GameObject นั้นกลับไปเลย
            }
        }

        return null; // วนจนจบแล้วไม่เจอ ส่งค่าว่างกลับไป
    }

    public void SetupItem(TrashData_Scene2 data)
    {
        displayImage = GetComponent<Image>();
        itemData = data;

        if (itemData.icon != null)
        {
            displayImage.sprite = itemData.icon;
        }
        else
        {
        }
    }

    private void KeepInsideScreen()
    {
        if (canvasGroup == null) return;

        // ดึงตำแหน่งปัจจุบันแบบ World Position มา
        Vector3 currentPos = rectTransform.position;

        // คำนวณขอบเขตหน้าจอในรูปแบบ World Space
        // (โดยสมมติว่าเมาส์อยู่ที่ขอบจอพอดี)
        float minX = 0;
        float maxX = Screen.width;
        float minY = 0;
        float maxY = Screen.height;

        // ถ้าระบบ UI คุณมี Pivot อยู่ตรงกลางไอเทม 
        // เราควรลบขนาดครึ่งหนึ่งของไอเทมออกด้วย เพื่อไม่ให้ขอบไอเทมล้นออกไป
        // แต่ถ้าเอาแค่จุดกึ่งกลางไม่หลุดจอ โค้ดด้านบนก้พอครับ

        // ถ้าอยากให้ขอบไอเทมไม่หลุดจอเลย ให้ใช้โค้ดชุดนี้แทน:
        /*
        float itemWidthHalf = rectTransform.rect.width * canvas.scaleFactor * 0.5f;
        float itemHeightHalf = rectTransform.rect.height * canvas.scaleFactor * 0.5f;
        minX += itemWidthHalf;
        maxX -= itemWidthHalf;
        minY += itemHeightHalf;
        maxY -= itemHeightHalf;
        */

        // ใช้สูตร Clamp: ตัดค่าไม่ให้เกิน min และ max ที่กำหนด
        currentPos.x = Mathf.Clamp(currentPos.x, minX, maxX);
        currentPos.y = Mathf.Clamp(currentPos.y, minY, maxY);

        // ใส่ตำแหน่งที่ถูกตัดขอบแล้วกลับไป
        rectTransform.position = currentPos;
    }
}

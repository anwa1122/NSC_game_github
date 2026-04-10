using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Reflection;
using Unity.VisualScripting;
using System.Collections.Generic;

public class DraggableTrash : MonoBehaviour, IBeginDragHandler , IDragHandler , IEndDragHandler
{
    public TrashData data; //ไปเอาข้อมูล item มา
    private RectTransform rectTransform; //ไปเอาตำแหน่งของ ตัว ui
    private CanvasGroup canvasGroup; // เอา component นีมาเพื่อจะใช้ให่้มันยิง raycast ได้ และสามารถลากได้
    public Vector3 startPosition; //ตำแหน่งเริ่มต้น
    public TrashType currentSlot; // slot ปัจจุบันหรอ

    private void Awake() 
    {
       
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData) 
    {
        
        canvasGroup.alpha = 0.6f;  //ทำให้มันล่องหน
        canvasGroup.blocksRaycasts = false; // ให้ยิงเลเซอร์ทะลุได้เอาไว้เช็คค่าต่างๆ
        transform.SetAsLastSibling(); // สั่งให้มันเป็นลูกตัวสุดท้ายอง parent นั้นๆเพื่อให้มันอยุ่ด้านบนสุดตลอดในภาพ ui
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition; //ให้ตำแหน่งตัว ui นี้เป็นไปตามตำแหน่งของเม้าส
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current); //กำหนดตัวแปรตำแหน่งจากเม้าส์หรอ?
        eventDataCurrentPosition.position = Input.mousePosition; // ให้ตัวแปรนั้น มีค่า่เป็นการคลิกค้างของเม้าส์ //ไม่แน่ใจตัวแปร

        List<RaycastResult> results = new List<RaycastResult>(); //สร้างเซ็ตข้อมูล เอาไว้ใฝช้ในการยิงเลเซอร์ทะลุแล้วเก็บข้อมูลเข้ามาในเซ็ต

        EventSystem.current.RaycastAll(eventDataCurrentPosition, results); //ยิงเลเศอร์ทะลุให้หมดเลย ที่ตำแหน่งเม้าส์ปัจจุบันที่คลิกค้างไว้ก้คือตัวแปร eventDataอันนั้น แล้วก้เอาไปเก็บในเซ็ต results

        currentSlot = TrashType.None; //รีเซ็ตอะไรสักอย่าง รีเซ็ต slot ปัจจุบันเพื่อให้ตอนลากต่อไปจะได้วางได้หรอ ไม่แน่ใจ

        foreach (RaycastResult result in results){ //ไปเอาข้อมูลในเซ็ตนั้นๆมาแต่ละตัวเริ่มจากอันแรกจนอันสุดท้าย

        if (result.gameObject.CompareTag("BeginSlot")) //ถ้าเจอ tag BeginSlot ในเซ็ตอันนั้น  ในที่นี้เอาไว้ใช้สำหรับตอนลากออกช่องแล้วดีดกลับ + การลากไปวางบน ui อันอื่นที่เป็นไอเทมเหมือนกัน เลยต้องใช้การ raycastAll
        {
            startPosition = transform.position;//ให้ตำแหน่งเริ่มต้นเท่ากับตำแหน่งปัจจุบันของตัวเอง
            break; // ออกจากลูป foreach อันนี้
        }
        }
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;   //ปรับให้มันทึบเหมือนเดิม
        canvasGroup.blocksRaycasts = true; // ปรับให้มันยิงเลเซอร์ไมทะลุแล้ว

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current); 
        eventDataCurrentPosition.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results); //เหมือนที่อธิบายไว้ด้านบน

        bool isFoundSlot = false; //ส้รางตัวแปรว่าเจอ slot มั้ย โดยกำหนดให้เปน flase ไปก่อนก้คือไม่เจอ

        foreach (RaycastResult result in results)  //ไปเอาข้อมูลในเซ็ตนั้นๆมาแต่ละตัวเริ่มจากอันแรกจนอันสุดท้าย ข้อมูลที่ยงเลเซอร์ไปได้
        {
            if (result.gameObject.CompareTag("TrashSlot")) //ถ้าเช็คแล้วเจอ tag ชื่อ TrashSlot 
            {
                
                isFoundSlot = true; //ให้เจอ slot เป็น true
                currentSlot = result.gameObject.GetComponent<TrashSlot>().acceptType; //ให้ currentslot เป็นตำแหน่ง result ที่มีแท็ก trashSlots
            }   
        }
            if (!isFoundSlot) //ถ้าไม่เจอ //การที่เราเอาไม่เจอไว้นอกลูปก้เพื่อ ใน foreach ถ้าเราเช็คไม่เจอทั้งหมดตัวแปร isFondSlot ก้จะเป็น false และมันก้หมายความว่าเราไมไ่ด้วางตรง TRashSlot ทำให้ต้องเด้งไปตำแหน่งเริ่ม้ตน
            {                   
                transform.position = startPosition; //ก้ให้กลับไปตำแหน่งเริ่มต้นมันที่ ิbeginslot
            }
        Object.FindFirstObjectByType<TrashMiniGameController>().UpdateConfirmButtonState();
    }
}
//ไม่แน่ใจเรื่อง currentSlot 
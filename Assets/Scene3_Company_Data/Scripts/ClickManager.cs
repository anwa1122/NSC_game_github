using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public static ClickManager Instance;
    public bool playerClick = false;
    public bool playerHold = false;
    public GameObject clickedObject;

    public Vector3 mouseWorldPosition;
    public Vector3 grabOffset; // เพิ่มตัวแปรเก็บระยะห่าง

    public List<GameObject> allTouchedObj;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        bool mouseHold = Input.GetMouseButton(0); //เม้าส์กดค้าง
        bool mouseDown = Input.GetMouseButtonDown(0); //เม้าส์คลิกรอบเดียว

        if (mouseHold || mouseDown) //ถ้าเม้าส์คลิกค้างหรือกดรอบเดียว
        {
            UpdateRaycast(mouseDown); //เรียกใช้ฟังชัน UpdateRaycast ให้ข้อมูล เม้าส์คลิกรอบเดียวไป เพราะในฟังชันมันเอาการคลิกครั้งแรก หรือเริ่มต้นคลิก
        }
        else
        {
            playerClick = false; //ถ้าไมไ่ด้กดอะไร ก้ให้คลิกเป็น false
            playerHold = false; //ให้คลิกค้างเป็น false
            clickedObject = null; //ให้ค่า clickedObject ให้มันเคลียๆไป
        }
    }

    void UpdateRaycast(bool isFirstClick) //ฟังชัน เรียกใช้ให้มันเก็บยค่าจากเม้าส์
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            mouseWorldPosition = hit.point;

            if (hit.collider.CompareTag("interactAble3D"))
            {

                if (isFirstClick)
                {
                    clickedObject = hit.collider.gameObject;
                    playerClick = true;

                    grabOffset = clickedObject.transform.position - hit.point;
                }

                if (Input.GetMouseButton(0))
                {
                    playerHold = true;
                }
            }
        }
    }
}
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public static ClickManager Instance;
    public bool playerClick = false;
    public bool playerHold = false;
    public GameObject clickedObject;

    public Vector3 mouseWorldPosition;
    public Vector3 grabOffset; // เพิ่มตัวแปรเก็บระยะห่าง

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
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            mouseWorldPosition = hit.point; //เก็บค่าตำแหน่งของเม้าส์

            if (hit.collider.CompareTag("interactAble3D")) //ถ้า raycast ยิงไปโดน tag ที่ชื่อ interactAble3D
            {
                if (isFirstClick) //ถ้าเป็นการคลิกครั้งแรก
                {
                    clickedObject = hit.collider.gameObject; //เก็บค่า gameObject ที่เม้าส์ยิงไปโดน
                    playerClick = true; //ให้ตัวแปร playerClick เป็นจริงเพื่อให้โค้ดอื่นเอาไปใช้ได้่ เอาไว้บอกว่า อันนี้ Player คลิกนะ ไม่ได้คลิกค้าง
                    
                    grabOffset = clickedObject.transform.position - hit.point; //ไม่แน่ใจ
                }

                if (Input.GetMouseButton(0))
                {
                    playerHold = true; //Player กดค้างเป็นจจริง
                }
            }
        }
    }
}
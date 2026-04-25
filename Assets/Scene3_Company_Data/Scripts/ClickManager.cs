using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public static ClickManager Instance;
    public bool playerClick = false;
    public GameObject clickedObject;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate
        }

        clickedObject = null;
    }
    void Update()
    {
        clickedObject = null;
        // 0 คือ คลิกซ้าย
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //ยิงเลเซอร์ออกไป
            RaycastHit hit; //ให้เลเซอร์ไปชนอะไรก้ได้ที่มันชนตัวแรก
            

            if (Physics.Raycast(ray, out hit)) 
            {
                Debug.Log("คุณคลิกโดน: " + hit.collider.gameObject.name);

                if (hit.collider.CompareTag("3DButton"))
                {
                    clickedObject = hit.collider.gameObject;
                    playerClick = true;
                }
            }
        }
        else
        {
            playerClick = false;
        }
    }
}

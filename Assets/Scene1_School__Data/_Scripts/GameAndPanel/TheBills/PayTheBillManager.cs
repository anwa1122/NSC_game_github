using UnityEngine;
using System.Collections; // อย่าลืมเพิ่มอันนี้เพื่อให้ใช้ Coroutine ได้

public class PayTheBillManager : MonoBehaviour
{
    public static PayTheBillManager Instance;
    [Header("Ui setting")]
    public GameObject payTheBillPanel;
    public CanvasGroup canvasGroupPassScene1;
    public float blinkSpeed = 1f; // ความเร็วในการกระพริบ

    [Header("BillScripts")]
    public ThisBill electricityBill;
    public ThisBill waterBill;
    public ThisBill FoodsBill;

    [Header("Won the game check")]
    public bool WinTheGame;

    private bool isBlinking = false; // เอาไว้เช็คไม่ให้รัน Coroutine ซ้ำ

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void OnExit()
    {
        payTheBillPanel.SetActive(false);
    }

    void Update()
    {
        if(WinTheGame)
        {
        }
        
    }
    public void CheckFullyPaid()
    {
        if (electricityBill.isFullyPaid && waterBill.isFullyPaid && FoodsBill.isFullyPaid)
        {
            PlayerDataManager.Instance.MoveToScene("Scene_2_Home");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        
    }

    IEnumerator BlinkEffect()
    {
        isBlinking = true;
        
        // มั่นใจว่าเปิด Object อยู่
        canvasGroupPassScene1.gameObject.SetActive(true);

        while (isBlinking)
        {
            // ใช้ Mathf.PingPong เพื่อให้ค่าเหวี่ยงจาก 0 ไป 1 และกลับมา 0 วนไปเรื่อยๆ
            // Time.time * blinkSpeed จะเป็นตัวกำหนดความเร็ว
            canvasGroupPassScene1.alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            
            yield return null; // รอเฟรมถัดไป
        }
    }
    
    // เผื่อไว้ใช้สั่งหยุดกระพริบ
    public void StopBlink()
    {
        isBlinking = false;
        StopCoroutine(BlinkEffect());
        canvasGroupPassScene1.alpha = 1f; // รีเซ็ตให้เข้มปกติ
    }
}
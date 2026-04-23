using UnityEngine;
using UnityEngine.UI;
public class ErrorBlock  : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 50f;      // ความเร็วในการเคลื่อนที่
    public float wanderRadius = 100f;  // รัศมีสูงสุดที่จะสุ่มไป (นับจากจุดเริ่มต้น)
    public float stopThreshold = 5f;   // ระยะห่างที่ถือว่าถึงจุดหมายแล้ว

    private Vector2 startPosition;     // จุดเริ่มต้น (เพื่อไม่ให้ลอยหายไปไกลเกิน)
    private Vector2 targetPosition;    // จุดหมายสุ่มถัดไป
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
        
        // สุ่มจุดหมายแรกทันทีที่เริ่มเกม
        SetNewRandomTarget();
    }

    void Update()
    {
        MoveTowardsTarget();

        // ถ้าเข้าใกล้จุดหมายแล้ว ให้สุ่มจุดหมายใหม่
        if (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) < stopThreshold)
        {
            SetNewRandomTarget();
        }
    }

    void MoveTowardsTarget()
    {
        // คำนวณการเคลื่อนที่ให้นุ่มนวลด้วย MoveTowards
        rectTransform.anchoredPosition = Vector2.MoveTowards(
            rectTransform.anchoredPosition, 
            targetPosition, 
            moveSpeed * Time.deltaTime
        );
    }

    void SetNewRandomTarget()
    {
        // สุ่มตำแหน่งใหม่ภายในรัศมีรอบๆ จุดเริ่มต้น
        float randomX = Random.Range(-wanderRadius, wanderRadius);
        float randomY = Random.Range(-wanderRadius, wanderRadius);
        
        targetPosition = startPosition + new Vector2(randomX, randomY);
    }

    public void LockAndMoveToTop()
{
    // 1. สั่งให้ Object นี้ไปอยู่อันดับบนสุดของ Child (ใน Hierarchy)
    // การเป็น Child ตัวแรกจะทำให้มันถูกวาดอยู่ข้างล่างสุด หรืออยู่อันดับแรกใน Layout
    transform.SetAsLastSibling();

    // 2. เปลี่ยนสีเป็นสีเขียว
    // ต้องมั่นใจว่า Object นี้มี Component Image แปะอยู่
    Image img = GetComponent<Image>();
    if (img != null)
    {
        img.color = Color.green;
    }

    // 3. ล็อคไม่ให้ขยับ (ปิดการทำงานของสคริปต์นี้)
    // เมื่อเราปิด enabled = false ฟังก์ชัน Update() จะหยุดทำงานทันที
    this.enabled = false;
    
    Debug.Log(gameObject.name + " has been locked and moved to top!");
}
}
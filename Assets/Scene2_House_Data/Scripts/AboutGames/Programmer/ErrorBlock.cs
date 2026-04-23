using UnityEngine;

public class UIWanderer : MonoBehaviour
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
}
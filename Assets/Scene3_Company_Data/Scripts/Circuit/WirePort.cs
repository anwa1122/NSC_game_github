using UnityEngine;

// วางไว้บน GameObject ที่เป็น port ของแต่ละ block
// ต้องมี Collider (เช่น SphereCollider) ติดอยู่ด้วยเพื่อให้ Raycast เจอ
[RequireComponent(typeof(Collider))]
public class WirePort : MonoBehaviour
{
    [Header("Port Info")]
    public string portName = "Port";
    public BlockNode parentBlock;    // block ที่ port นี้สังกัดอยู่

    private bool isHovered = false;
    private Renderer rend;
    private Color originalColor;

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color hoverColor = Color.cyan;
    public Color draggingColor = Color.yellow;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            // สร้างสีเริ่มต้นแบบโปร่งแสง (ค่า A คือความใส 0.0 - 1.0)
            // สมมติให้ตอนแรกใสมากๆ (0.2f)
            normalColor.a = 0.1f;

            // อย่าลืมปรับสี Hover และ Dragging ให้มีความใสตามที่ต้องการด้วยนะจ๊ะ
            hoverColor.a = 0.8f;
            draggingColor.a = 1.0f;

            rend.material.color = normalColor;
            originalColor = normalColor;
        }
    }
    void OnMouseDown()
    {
        // เริ่ม drag จาก port นี้
        WireConnector.Instance.StartDragging(this);
        if (rend != null) rend.material.color = normalColor;
    }

    void OnMouseEnter()
    {
        isHovered = true;
        if (rend != null) rend.material.color = normalColor;
    }

    void OnMouseExit()
    {
        isHovered = false;
        if (rend != null) rend.material.color = normalColor;
    }
}

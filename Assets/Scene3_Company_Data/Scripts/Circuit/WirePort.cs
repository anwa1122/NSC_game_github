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
    public Color normalColor  = Color.white;
    public Color hoverColor   = Color.cyan;
    public Color draggingColor = Color.yellow;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = normalColor;
            originalColor = normalColor;
        }
    }

    void OnMouseDown()
    {
        // เริ่ม drag จาก port นี้
        WireConnector.Instance.StartDragging(this);
        if (rend != null) rend.material.color = draggingColor;
    }

    void OnMouseEnter()
    {
        isHovered = true;
        if (rend != null) rend.material.color = hoverColor;
    }

    void OnMouseExit()
    {
        isHovered = false;
        if (rend != null) rend.material.color = normalColor;
    }
}

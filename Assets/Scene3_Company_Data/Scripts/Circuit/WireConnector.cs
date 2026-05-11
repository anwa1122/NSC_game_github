using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WireConnector : MonoBehaviour
{
    public static WireConnector Instance;

    [Header("=== โหมดการแสดงผล ===")]
    public bool useObjectInsteadOfLine = false; // ✅ เปิดเพื่อใช้โมเดล 3D แทน LineRenderer
    public GameObject wirePrefab; // ลาก Cylinder หรือโมเดลที่ต้องการใส่ตรงนี้

    [Header("=== สไตล์เส้น ===")]
    public WireStyle wireStyle = WireStyle.Straight;   // รูปแบบการหักมุม (ใช้ได้เฉพาะ LineRenderer)

    [Header("=== ขนาดและสี ===")]
    public float wireWidth = 0.05f;               // ความหนาของเส้น
    public Color wireColor = Color.yellow;         // สีเส้นปกติ
    public Color previewColor = new Color(1f, 1f, 0f, 0.4f); // สีตอนกำลังลาก
    public Material wireMaterial;                    // Material (ไม่ใส่ = default)

    [Header("=== จุดหักมุม ===")]
    [Range(0f, 1f)]
    public float bendPosition = 0.5f;               // จุดที่เส้นจะหักมุม (0=ชิดต้นทาง, 1=ชิดปลายทาง)

    [Header("=== ความเรียบ (เฉพาะ Bezier) ===")]
    [Range(5, 40)]
    public int curveResolution = 20;                // ความละเอียดเส้นโค้ง

    // ---- private ----
    private LineRenderer previewLine;
    private GameObject previewWireObject; // โมเดลตอนลาก
    private List<WireConnection> connections = new List<WireConnection>();
    private WirePort draggingFrom = null;
    private bool isDragging = false;

    public enum WireStyle
    {
        LShape,      // หักมุมครั้งเดียว (แบบ L)
        ZShape,      // หักมุมสองครั้ง (แบบ Z หรือ S)
        Straight,    // เส้นตรง
        Bezier,      // โค้ง Bezier
    }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        previewLine = GetComponent<LineRenderer>();
        SetupLineRenderer(previewLine, previewColor);
        previewLine.enabled = false;
    }

    void Update()
    {
        if (!isDragging || draggingFrom == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(-Camera.main.transform.forward, draggingFrom.transform.position);
        if (plane.Raycast(ray, out float dist))
        {
            Vector3 mouseWorld = ray.GetPoint(dist);

            // เลือกแสดงผลตามโหมด
            if (useObjectInsteadOfLine && previewWireObject != null)
            {
                UpdateWireObject(previewWireObject, draggingFrom.transform.position, mouseWorld);
            }
            else
            {
                DrawWire(previewLine, draggingFrom.transform.position, mouseWorld);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            TryConnectToPort();
            StopDragging();
        }
    }

    public void StartDragging(WirePort fromPort)
    {
        draggingFrom = fromPort;
        isDragging = true;

        if (useObjectInsteadOfLine && wirePrefab != null)
        {
            // สร้างโมเดล preview
            previewWireObject = Instantiate(wirePrefab);
            previewWireObject.name = "PreviewWire";
            SetObjectColor(previewWireObject, previewColor);
        }
        else
        {
            // ใช้ LineRenderer แบบเดิม
            previewLine.enabled = true;
        }
    }

    void TryConnectToPort()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        foreach (var hit in Physics.RaycastAll(ray, 100f))
        {
            WirePort target = hit.collider.GetComponent<WirePort>();
            if (target != null && target != draggingFrom)
            {
                CreateConnection(draggingFrom, target);
                return;
            }
        }
    }

    void StopDragging()
    {
        isDragging = false;
        draggingFrom = null;

        // ลบโมเดล preview
        if (previewWireObject != null)
        {
            Destroy(previewWireObject);
            previewWireObject = null;
        }

        previewLine.enabled = false;
    }

    public void CreateConnection(WirePort from, WirePort to)
    {
        foreach (var c in connections)
            if ((c.from == from && c.to == to) || (c.from == to && c.to == from)) return;

        if (useObjectInsteadOfLine && wirePrefab != null)
        {
            // ใช้โมเดล 3D
            GameObject wireObj = Instantiate(wirePrefab);
            wireObj.name = $"Wire_{from.name}_{to.name}";
            UpdateWireObject(wireObj, from.transform.position, to.transform.position);
            SetObjectColor(wireObj, wireColor);

            connections.Add(new WireConnection
            {
                from = from,
                to = to,
                wireObject = wireObj
            });
        }
        else
        {
            // ใช้ LineRenderer แบบเดิม
            GameObject wireObj = new GameObject($"Wire_{from.name}_{to.name}");
            LineRenderer lr = wireObj.AddComponent<LineRenderer>();
            SetupLineRenderer(lr, wireColor);
            DrawWire(lr, from.transform.position, to.transform.position);

            connections.Add(new WireConnection
            {
                from = from,
                to = to,
                lineRenderer = lr
            });
        }

        Debug.Log($"[Wire] {from.name} → {to.name}");
    }

    public void RemoveAllConnections()
    {
        foreach (var c in connections)
        {
            if (c.lineRenderer != null) Destroy(c.lineRenderer.gameObject);
            if (c.wireObject != null) Destroy(c.wireObject);
        }
        connections.Clear();
    }

    // ---- อัปเดตตำแหน่งและขนาดของโมเดล 3D ----
    // ---- อัปเดตตำแหน่งและขนาดของโมเดล 3D ----
    // ---- อัปเดตตำแหน่งและขนาดของโมเดล 3D ----
    // ---- อัปเดตตำแหน่งและขนาดของโมเดล 3D ----
    void UpdateWireObject(GameObject obj, Vector3 start, Vector3 end)
    {
        float distance = Vector3.Distance(start, end);
        Vector3 direction = (end - start).normalized;

        // วางที่จุดเริ่มต้น
        obj.transform.position = start;

        // หมุนให้ชี้ไปหาจุดปลายทาง
        obj.transform.rotation = Quaternion.LookRotation(direction);

        // ===== ลองวิธีนี้ก่อน (แกนยาว = X) =====
        obj.transform.localScale = new Vector3(
            distance,         // ความยาว X
            wireWidth * 10f,  // ความหนา Y
            wireWidth * 10f   // ความหนา Z
        );

        // ถ้าไม่ได้ ลบ 4 บรรทัดบนแล้วลองนี้แทน:

        // ===== วิธีที่ 2: แกนยาว = Y =====
        /*
        obj.transform.localScale = new Vector3(
            wireWidth * 10f,  // ความหนา X
            distance,         // ความยาว Y
            wireWidth * 10f   // ความหนา Z
        );
        obj.transform.Rotate(0f, 90f, 0f); // หมุนเพิ่มถ้าทิศทางไม่ตรง
        */

        // ===== วิธีที่ 3: แกนยาว = Z =====
        /*
        obj.transform.localScale = new Vector3(
            wireWidth * 10f,  // ความหนา X
            wireWidth * 10f,  // ความหนา Y
            distance          // ความยาว Z
        );
        */

        // ถ้า Pivot อยู่ตรงกลาง ให้เลื่อนไปครึ่งทาง
        obj.transform.position = start + direction * (distance / 2f);
    }

    // ---- ตั้งค่าสีให้โมเดล ----
    void SetObjectColor(GameObject obj, Color color)
    {
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null)
        {
            // สร้าง Material ใหม่เพื่อไม่ให้กระทบ Prefab เดิม
            rend.material = new Material(rend.material);
            rend.material.color = color;
        }
    }

    // ---- วาดเส้นตามสไตล์ที่เลือก (สำหรับ LineRenderer) ----
    void DrawWire(LineRenderer lr, Vector3 start, Vector3 end)
    {
        switch (wireStyle)
        {
            case WireStyle.LShape: DrawLShape(lr, start, end); break;
            case WireStyle.ZShape: DrawZShape(lr, start, end); break;
            case WireStyle.Straight: DrawStraight(lr, start, end); break;
            case WireStyle.Bezier: DrawBezier(lr, start, end); break;
        }
    }

    // L-shape: ไปแนวนอนก่อน แล้วหักขึ้น/ลง
    void DrawLShape(LineRenderer lr, Vector3 s, Vector3 e)
    {
        float midX = Mathf.Lerp(s.x, e.x, bendPosition);
        Vector3 corner = new Vector3(midX, s.y, s.z + (e.z - s.z) * 0f);
        Vector3 mid = new Vector3(midX, s.y, e.z);
        SetPoints(lr, s, mid, e);
    }

    // Z-shape: หักสองครั้ง (แบบบันได)
    void DrawZShape(LineRenderer lr, Vector3 s, Vector3 e)
    {
        float midX = Mathf.Lerp(s.x, e.x, bendPosition);
        Vector3 p1 = new Vector3(midX, s.y, s.z);
        Vector3 p2 = new Vector3(midX, e.y, e.z);
        SetPoints(lr, s, p1, p2, e);
    }

    // เส้นตรง
    void DrawStraight(LineRenderer lr, Vector3 s, Vector3 e)
    {
        SetPoints(lr, s, e);
    }

    // Bezier โค้งนิดหน่อย
    void DrawBezier(LineRenderer lr, Vector3 s, Vector3 e)
    {
        Vector3 dir = (e - s);
        Vector3 ctrl1 = s + new Vector3(dir.x * 0.5f, 0, 0);
        Vector3 ctrl2 = e - new Vector3(dir.x * 0.5f, 0, 0);

        lr.positionCount = curveResolution;
        for (int i = 0; i < curveResolution; i++)
        {
            float t = i / (float)(curveResolution - 1);
            float u = 1 - t;
            lr.SetPosition(i, u * u * u * s + 3 * u * u * t * ctrl1 + 3 * u * t * t * ctrl2 + t * t * t * e);
        }
    }

    // helper ใส่จุดใน LineRenderer
    void SetPoints(LineRenderer lr, params Vector3[] pts)
    {
        lr.positionCount = pts.Length;
        for (int i = 0; i < pts.Length; i++) lr.SetPosition(i, pts[i]);
    }

    void SetupLineRenderer(LineRenderer lr, Color color)
    {
        lr.startWidth = wireWidth;
        lr.endWidth = wireWidth;
        lr.useWorldSpace = true;
        lr.startColor = color;
        lr.endColor = color;
        lr.material = wireMaterial != null
            ? wireMaterial
            : new Material(Shader.Find("Sprites/Default"));
    }
}

[System.Serializable]
public class WireConnection
{
    public WirePort from;
    public WirePort to;
    public LineRenderer lineRenderer;
    public GameObject wireObject; // เพิ่มตัวนี้เพื่อเก็บโมเดล 3D
}
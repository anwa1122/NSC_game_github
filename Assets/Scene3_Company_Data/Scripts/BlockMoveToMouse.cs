using UnityEngine;

public class BlockMoveToMouse : MonoBehaviour
{
    public Transform blockTransform;
    public bool playerHold = false;

    private float posX;
    private float posZ;
    void Update()
    {

    }

    void LateUpdate()
    {
        // ถ้า Manager บอกว่ากำลังลาก "ฉัน" อยู่
        if (ClickManager.Instance.playerHold && ClickManager.Instance.clickedObject == this.gameObject)
        {
            // ตำแหน่งเป้าหมาย = ตำแหน่งเมาส์ปัจจุบัน + ระยะห่างตอนเริ่มคลิก                                 
            Vector3 targetPos = ClickManager.Instance.mouseWorldPosition + ClickManager.Instance.grabOffset;

            // ล็อกแกน Y ไว้
            targetPos.y = transform.position.y;

            //ตำแหน่งของวัตถุตัวนี้
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.2f);
        }
    }
}

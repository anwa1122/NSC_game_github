using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class CircuitManager : MonoBehaviour
{
    public static CircuitManager Instance;

    public List<DeviceType> allDevice; // เก็บประวัติการต่อทั้งหมด (ถ้ายังจำเป็นต้องใช้)
    public List<DeviceType> remainDevice; // ลำดับอุปกรณ์ปัจจุบันที่จะโชว์บนหน้าจอ

    public TextMeshProUGUI text;
    public Transform wireSpaceObj; // ตัวเก็บสายไฟ (ลูกๆ)

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // --- 🦋 Hu Tao บอกว่า: เอา Distinct ออกจาก Update ไปเลยนะจ๊ะ! 🦋 ---
    void Update()
    {
        // ปล่อยว่างไว้ หรือเอาไว้ใช้ทำอย่างอื่นที่ไม่ใช่การจัดการ List นี้จ้ะ
    }

    public void RefreshRemainDevice()
    {
        remainDevice.Clear();

        // สร้าง List ชั่วคราวเพื่อเก็บคู่การเชื่อมต่อที่ตรวจสอบแล้ว
        List<string> confirmedConnections = new List<string>();

        foreach (Transform child in wireSpaceObj)
        {
            ConnectionInfo info = child.GetComponent<ConnectionInfo>();
            if (info == null) continue;

            // สร้าง ID พิเศษเพื่อเช็คคู่ เช่น "Battery_3V-Switch"
            string connectionPair = info.firstDevice.ToString() + "-" + info.secondDevice.ToString();

            // เช็คว่าในบรรดาลูกๆ (สายไฟเส้นอื่น) มีเส้นที่เชื่อมคู่เดียวกันแต่คนละสีไหม
            bool hasPartner = false;
            foreach (Transform otherChild in wireSpaceObj)
            {
                ConnectionInfo otherInfo = otherChild.GetComponent<ConnectionInfo>();
                if (otherInfo != null && otherChild != child) // ไม่เช็คตัวเอง
                {
                    // ถ้าเชื่อมอุปกรณ์เดียวกัน แต่อีกเส้นเป็นคนละสี (เช่น แดง กับ ดำ)
                    if (otherInfo.firstDevice == info.firstDevice &&
                        otherInfo.secondDevice == info.secondDevice &&
                        otherInfo.nodeType != info.nodeType)
                    {
                        hasPartner = true;
                        break;
                    }
                }
            }

            // ถ้าเจอคู่ (แดง+ดำ) และเรายังไม่เคยบันทึกคู่นี้ลงไป
            if (hasPartner && !confirmedConnections.Contains(connectionPair))
            {
                if (remainDevice.Count == 0)
                {
                    remainDevice.Add(info.firstDevice);
                }
                remainDevice.Add(info.secondDevice);

                // จดไว้ว่าคู่นี้ประทับตราเรียบร้อยแล้ว จะได้ไม่นับซ้ำจากสายอีกเส้นในคู่
                confirmedConnections.Add(connectionPair);
            }
        }

        UpdateDisplayText();
    }

    private void UpdateDisplayText()
    {
        Debug.Log(text);
        if (text != null)
        {
            // แปลงลิสต์เป็นข้อความสวยๆ เช่น "Battery -> Switch -> LED"
            Debug.Log("ez");
            text.text = string.Join(" -> ", remainDevice);
        }
    }

    // ฟังก์ชันสำหรับปุ่ม Undo ในเกม
    public void removeDevice()
    {
        // แค่สั่ง Undo สายไฟ แล้วระบบ Refresh จะจัดการที่เหลือเองจ้ะ!
        UndoLastWire();
    }

    public void UndoLastWire()
    {
        int childCount = wireSpaceObj.childCount;
        if (childCount > 0)
        {
            // ทำลายวัตถุสายไฟล่าสุด
            // ใช้ Destroy ปกติถ้าเรียกตอนรันเกมทั่วไปนะจ๊ะ
            Destroy(wireSpaceObj.GetChild(childCount - 1).gameObject);

            // รอจบเฟรมแล้วค่อยรีเฟรช หรือใช้ Invoke ก็ได้ 
            // แต่ในที่นี้เราเรียก Refresh ต่อท้ายไปเลยเพื่อให้ List อัปเดตทันที
            // *หมายเหตุ: ถ้าใช้ Destroy ธรรมดา childCount จะยังไม่ลดทันทีในเฟรมนั้น 
            // อาจจะต้องรอ 0.1 วินาทีแล้วค่อย Refresh หรือจัดการ List แยกจ้ะ

            // แนะนำ: ให้ประกาศฟังก์ชันนี้เป็น Coroutine หรือเรียกหลังจาก Destroy 1 เฟรม
            Invoke("RefreshRemainDevice", 0.05f);
        }
        else
        {
            // ถ้าสายไฟหมดแล้ว ก็ล้างลิสต์โชว์ให้ว่างเปล่า
            remainDevice.Clear();
            UpdateDisplayText();
        }
    }
}
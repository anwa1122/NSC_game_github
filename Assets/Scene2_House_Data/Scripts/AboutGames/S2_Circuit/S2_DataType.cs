using UnityEngine;
using System.Collections.Generic;
// สคริปต์นี้ไม่ต้องมี MonoBehaviour
public enum S2_DeviceType
{
    None,
    Battery_3V,
    Battery_9V,
    Battery_12V,
    Switch,
    Resistor,
    LED,
    Motor,
    Servo
}

public enum S2_DeviceClass
{
    Battery,
    Controller,
    Output
}

public enum S2_NodeClass
{
    Send,
    Get
}

public enum S2_NodeType
{
    Red, // ขั้วบวก (แดง)
    Black, // ขั้วลบ (ดำ)
    Signal    // สายสัญญาณ (เหลือง)
}

public class S2_DataType : MonoBehaviour
{
    public static S2_DeviceClass GetDeviceClass(S2_DeviceType type)
    {
        switch (type)
        {
            case S2_DeviceType.Battery_3V:
            case S2_DeviceType.Battery_9V:
            case S2_DeviceType.Battery_12V:
                return S2_DeviceClass.Battery;

            case S2_DeviceType.Switch:
                return S2_DeviceClass.Controller;

            case S2_DeviceType.LED:
            case S2_DeviceType.Motor:
            case S2_DeviceType.Servo:
                return S2_DeviceClass.Output;

            default:
                return S2_DeviceClass.Controller;
        }
    }

    public static S2_DeviceType GetRandomDeviceByClass(S2_DeviceClass targetClass)
    {
        // 1. สร้างลิสต์ชั่วคราวเพื่อเก็บอุปกรณ์ที่อยู่ในกลุ่มที่ต้องการ
        List<S2_DeviceType> candidates = new List<S2_DeviceType>();

        // 2. วนลูปหาว่า DeviceType ไหนบ้างที่ตรงกับกลุ่มที่เราอยากได้
        foreach (S2_DeviceType type in System.Enum.GetValues(typeof(S2_DeviceType)))
        {
            if (type == S2_DeviceType.None) continue; // ข้าม None ไปจ้ะ

            // ใช้ฟังก์ชันแยกประเภทที่เราเขียนไว้ก่อนหน้ามาช่วยเช็ค
            if (GetDeviceClass(type) == targetClass)
            {
                candidates.Add(type);
            }
        }

        // 3. สุ่มจากลิสต์ที่เราคัดมาแล้ว
        if (candidates.Count > 0)
        {
            int randomIndex = Random.Range(0, candidates.Count);
            return candidates[randomIndex];
        }

        return S2_DeviceType.None; // ถ้าไม่เจอใครเลย
    }
}

using UnityEngine;
using System.Collections.Generic;
// สคริปต์นี้ไม่ต้องมี MonoBehaviour
public enum DeviceType
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

public enum DeviceClass
{
    Battery,
    Controller,
    Output
}

public enum NodeClass
{
    Send,
    Get
}

public enum NodeType
{
    Red, // ขั้วบวก (แดง)
    Black, // ขั้วลบ (ดำ)
    Signal    // สายสัญญาณ (เหลือง)
}

public class DataType : MonoBehaviour
{
    public static DeviceClass GetDeviceClass(DeviceType type)
    {
        switch (type)
        {
            case DeviceType.Battery_3V:
            case DeviceType.Battery_9V:
            case DeviceType.Battery_12V:
                return DeviceClass.Battery;

            case DeviceType.Switch:
                return DeviceClass.Controller;

            case DeviceType.LED:
            case DeviceType.Motor:
            case DeviceType.Servo:
                return DeviceClass.Output;

            default:
                return DeviceClass.Controller;
        }
    }

    public static DeviceType GetRandomDeviceByClass(DeviceClass targetClass)
    {
        // 1. สร้างลิสต์ชั่วคราวเพื่อเก็บอุปกรณ์ที่อยู่ในกลุ่มที่ต้องการ
        List<DeviceType> candidates = new List<DeviceType>();

        // 2. วนลูปหาว่า DeviceType ไหนบ้างที่ตรงกับกลุ่มที่เราอยากได้
        foreach (DeviceType type in System.Enum.GetValues(typeof(DeviceType)))
        {
            if (type == DeviceType.None) continue; // ข้าม None ไปจ้ะ

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

        return DeviceType.None; // ถ้าไม่เจอใครเลย
    }
}

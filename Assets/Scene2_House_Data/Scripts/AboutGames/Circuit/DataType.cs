using UnityEngine;

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

}

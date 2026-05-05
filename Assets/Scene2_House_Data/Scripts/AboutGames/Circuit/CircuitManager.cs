using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class CircuitManager : MonoBehaviour
{
    public static CircuitManager Instance;

    public List<DeviceType> allDevice;

    private DeviceType lastDevice = DeviceType.None;
    void Awake()
    {
        if (Instance == null) Instance = this;
        lastDevice = DeviceType.None;
    }

    public void addDevice(DeviceType newDevice)
    {
        if (lastDevice != newDevice)
        {
            allDevice.Add(newDevice);
            lastDevice = newDevice;
        }

    }
}
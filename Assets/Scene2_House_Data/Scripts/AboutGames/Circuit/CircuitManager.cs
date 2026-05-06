using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.iOS;
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
        allDevice.Add(newDevice);
    }

    public void removeDevice()
    {
        if (allDevice.Count < 1) return;

        allDevice.RemoveAt(allDevice.Count - 1);

        if (allDevice.Count < 1) return;
        allDevice.RemoveAt(allDevice.Count - 1);
    }
}
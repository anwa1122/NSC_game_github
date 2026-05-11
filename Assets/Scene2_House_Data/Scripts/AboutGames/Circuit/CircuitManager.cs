using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEditor.PackageManager.Requests;

public class CircuitManager : MonoBehaviour
{
    public static CircuitManager Instance;

    [Header("QuestName")]
    public GameType questType;

    [Header("Other")]
    public int componentCount;

    [Header("Get Device (Blanked Only)")]
    public List<DeviceType> remainDevice; // ลำดับอุปกรณ์ปัจจุบันที่จะโชว์บนหน้าจอ
    public List<DeviceType> requestDevice;

    [Header("Ui management")]
    public GameObject circuitGamePanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI debugText;
    public TextMeshProUGUI requestText;
    public Transform wireSpaceObj; // ตัวเก็บสายไฟ (ลูกๆ)

    [Header("End game bool")]
    public bool completeCircuitGame = false;

    private bool addMoney = false;
    private int score;

    private QuestData thisQuestData = null;
    private DeviceType batteryDevice;
    private DeviceType controlDevice;
    private DeviceType outputDevice;

    void Awake()
    {
        if (Instance == null) Instance = this;
        StartCircuitgame();
    }




    public void StartCircuitgame()
    {
        if (requestText != null)
        {
            scoreText.text = "Score : " + score;
            RandomDeviceType();
            requestText.text = $"i want to connect {batteryDevice} through {controlDevice} and then {outputDevice}";

            requestDevice.Add(batteryDevice);
            requestDevice.Add(controlDevice);
            requestDevice.Add(outputDevice);

            debugText.text = "None";
        }
    }

    public void RandomDeviceType()
    {
        batteryDevice = DataType.GetRandomDeviceByClass(DeviceClass.Battery);
        controlDevice = DataType.GetRandomDeviceByClass(DeviceClass.Controller);
        outputDevice = DataType.GetRandomDeviceByClass(DeviceClass.Output);
    }

    public void checkResult()
    {
        if (completeCircuitGame) return;
        for (int i = 0; i < componentCount; i++)
        {
            if (3 > remainDevice.Count || componentCount > requestDevice.Count)
            {
                return;
            }
            else if (requestDevice[i] == remainDevice[i])//e
            {
                score += 10;
            }
            else
            {
                score -= 5;
            }
        }

        scoreText.text = "Score : " + score;
        completeCircuitMinigame();
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
        if (debugText != null)
        {
            debugText.text = string.Join(" -> ", remainDevice);
        }
    }

    public void UndoLastWire()
    {
        if (completeCircuitGame) return;

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

    public void ExitGame()
    {
        PC_SystemManager.Instance.ExitWindow(circuitGamePanel);
    }

    public void completeCircuitMinigame()
    {
        completeCircuitGame = true;

        FreelanceHubManager.Instance.GetQuestData(questType, out thisQuestData);

        if (!addMoney)
        {
            addMoney = true;
            FreelanceHubManager.Instance.RemoveQuest(questType);
            if (PlayerMoneyTest_Scene2.Instance == null) return;
            PlayerMoneyTest_Scene2.Instance.AddMoney(score / 10 * thisQuestData.baseReward);

        }
    }
}
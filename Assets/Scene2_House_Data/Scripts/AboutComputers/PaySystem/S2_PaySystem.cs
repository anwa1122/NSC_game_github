using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // เพิ่มอันนี้มาเพื่อเปลี่ยนฉาก
using System.Collections;
using System.Collections.Generic; // เพิ่มอันนี้มาเพื่อใช้ Coroutine

public class S2_PaySystem : MonoBehaviour
{
    public static S2_PaySystem Instance;
    [Header("UI Elements")]
    public GameObject payListPanel;

    [Header("Bills")]
    public List<S2_EachPayment> eachPayment;

    [Header("Win the game script")]
    public S2_WinTheGameScript winScript;

    public int allScriptsCount;
    public int paidAllCount;

    void Awake()
    {
        if (Instance == null) Instance = this;

        allScriptsCount = eachPayment.Count;
    }

    public void AreAllBillsCleared()
    {
        paidAllCount = 0;
        foreach (S2_EachPayment script in eachPayment)
        {
            if (script.paidAll)
            {
                paidAllCount++;
            }
        }

        if (paidAllCount == allScriptsCount)
        {
            S2_NotificationManager.Instance.showNotificationWithSetTime("You have cleared bills!", 5f);
        }
    }
    public void ExitPayUi()
    {
        S2_PC_SystemManager.Instance.ExitWindow(payListPanel);
        S2_NotificationManager.Instance.stopNotification();
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // เพิ่มอันนี้มาเพื่อเปลี่ยนฉาก
using System.Collections; // เพิ่มอันนี้มาเพื่อใช้ Coroutine

public class PaySystem_Scene2 : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject payListPanel;
    public TextMeshProUGUI costText;
    public Button payButton;          // ลากปุ่มที่ใช้จ่ายเงินมาใส่

    [Header("Other settings")]
    public float price;
    [Header("Win the game script")]
    public WinTheGameScript winScript;

    void OnEnable()
    {
        costText.text = "Price : " + price;
        Debug.Log("Enabled");
    }

    public void playerPayMoney()
    {
        if (PlayerDataManager.Instance.money >= price)
        {
            PlayerDataManager.Instance.money -= price;
            
            // 1. ทำให้ปุ่มกดไม่ได้ทันที
            payButton.interactable = false;

            // 2. เริ่มกระบวนการจอดำและเปลี่ยนฉากe
            winScript.PlayerPassScene();
        }
    }
    public void ExitPayUi()
    {
        PC_SystemManager.Instance.ExitWindow(payListPanel);
    }
}
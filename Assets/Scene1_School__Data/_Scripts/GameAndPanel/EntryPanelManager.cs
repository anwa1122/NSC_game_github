using UnityEngine;
using System.Collections.Generic;
using System;
// 1. เพิ่ม , IResettable
public class EntryPanelManager : MonoBehaviour, IResettable
{
    public SitSystem sitSystem;
    public GameObject sortingGamePanel;
    public GameObject payTheBillPanel;
    public TrashMiniGameController miniGameController;

    [Header("Text")]
    public String text = "You have already Done this";
    public String alreadyDoneText = "You have already Done this";
    public String morningText = "You can't do sorting in the Morning";
    
    [SerializeField]
    public bool onPlayerSitting;
    
    [SerializeField]
    public GameObject enterGameUI;


    // 2. ฟังก์ชันรีเซ็ตตามกฎ Interface
    public void ResetObject()
    {
        // ปิดหน้าจอ UI ทั้งหมดที่มีโอกาสค้าง
        if (enterGameUI != null) enterGameUI.SetActive(false);
        if (sortingGamePanel != null) sortingGamePanel.SetActive(false);
        if (payTheBillPanel != null) payTheBillPanel.SetActive(false);

        // คืนค่าเมาส์ให้กลับไปล็อก (เผื่อกรณีตื่นมาแล้วต้องเดินเลย)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("EntryPanel UI has been cleaned for the new day.");
    }

    public void SetSittingState(bool isSitting)
    {
        if (isSitting)
        {
            enterGameUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            enterGameUI.SetActive(false);
            sortingGamePanel.SetActive(false);
        }
    }

    public void StartTheGame()
    {
        if (!miniGameController.winTheGame && GameGlobal.Instance.currentPhase == DayPhase.Afternoon)
        {
            sortingGamePanel.SetActive(true);
            miniGameController.SetUpMiniGame();
            CantDo_Text.Instance.showAlreadyDoneText(text,false);
        }
        else
        {
            if (miniGameController.winTheGame)text = alreadyDoneText;
            if(!(GameGlobal.Instance.currentPhase == DayPhase.Afternoon))text = morningText;
            CantDo_Text.Instance.showAlreadyDoneText(text,true);
        }
        
    }

    public void StartTheBillPaying()
    {
        payTheBillPanel.SetActive(true);
    }

    public void ExitTheGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        enterGameUI.SetActive(false);
        sortingGamePanel.SetActive(false);
        sitSystem.exitGame();
    }
}
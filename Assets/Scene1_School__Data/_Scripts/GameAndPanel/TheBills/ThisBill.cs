using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro; // ต้องมีเพื่อใช้ List
public class ThisBill : MonoBehaviour
{
    [Header("Ui Settings")]
    public List<Image> stepSlots = new List<Image>();
    public TextMeshProUGUI payCostText;

    [Header("Config")]
    public float basePrice = 50f;
    public float priceAddPerStep = 5f;

    [HideInInspector]
    public bool isFullyPaid = false;
    private int currentStep = 0;

    void Awake()
    {
        payCostText.text = "Pay (" + basePrice + ")";
    }
    public void payTheBill()
    {
        float playerMoney = PlayerDataManager.Instance.money;
        if (playerMoney >= basePrice && isFullyPaid == false)
        {
            PlayerDataManager.Instance.money = playerMoney - basePrice;

            if (currentStep <= stepSlots.Count)
            {
                increaseBasePrice();
                stepSlots[currentStep].color = new Color(0.2f, 1f, 0.2f, 1f);
                currentStep++;
                if (currentStep == stepSlots.Count)
                {
                    isFullyPaid = true;
                    PayTheBillManager.Instance.CheckFullyPaid();
                }
            }
        }
        
    }

    public void increaseBasePrice()
    {
        basePrice = basePrice + priceAddPerStep;
        payCostText.text = "Pay (" + basePrice + ")";
    }
}

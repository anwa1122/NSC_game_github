using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;
public class S2_EachPayment : MonoBehaviour
{
    [Header("Ui Settings")]
    public TextMeshProUGUI priceText;
    public Transform slot;
    [Header("Payment Setting")]
    public float price;
    public float upgradePrice;
    public bool paidAll = false;

    private int childCount;
    private int childGreen;

    void Start()
    {
        priceText.text = "Price : " + price;
        childCount = slot.childCount;
    }

    public void payThisBill()
    {
        if (S2_PlayerMoneyTest.Instance.playerMoney >= price)
        {
            childGreen = 0;

            S2_PlayerMoneyTest.Instance.SubtractMoney(price);

            //S2_PaySystem.Instance.playerPayMoney();
            foreach (Transform child in slot)
            {
                Image image = child.GetComponent<Image>();

                if (image.color != Color.green)
                {
                    image.color = Color.green;
                    price = price * upgradePrice;
                    priceText.text = "Price : " + price;
                    childGreen += 1;
                    break;
                }
                else
                {
                    childGreen += 1;
                }
            }

            if (childCount == childGreen)
            {
                paidAll = true;
                S2_PaySystem.Instance.AreAllBillsCleared();
            }
        }
        else if (!paidAll)
        {
            S2_NotificationManager.Instance.showNotificationNormally("Not enough money");
        }
        else
        {
            S2_NotificationManager.Instance.showNotificationNormally("You have already paid this");
        }
    }
}

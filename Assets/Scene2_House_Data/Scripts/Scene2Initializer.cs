using TMPro;
using UnityEngine;

public class Scene2Initializer : MonoBehaviour
{
    public TextMeshProUGUI localMoneyText;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // ส่งตัว UI Text ในฉากนี้ไปให้ PlayerDataManager ที่ตามมาจากฉาก 1
        if (PlayerDataManager.Instance != null)
        {
            PlayerDataManager.Instance.UpdateMoneyTextReference(localMoneyText);
        }
    }
}
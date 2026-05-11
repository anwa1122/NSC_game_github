using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMoneyTest_Scene2 : MonoBehaviour
{
    public static PlayerMoneyTest_Scene2 Instance;
    public TextMeshProUGUI playerMoneytext;
    public float playerMoney;
    void Start()
    {
        if (Instance == null) Instance = this;
        playerMoneytext.text = "PlayerMoney : " + playerMoney;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddMoney(float getMoney)
    {
        playerMoney += getMoney;
        playerMoneytext.text = "PlayerMoney : " + playerMoney;
        Debug.Log(getMoney);
        //PlayerDataManager.Instance.money += getMoney;
    }
}

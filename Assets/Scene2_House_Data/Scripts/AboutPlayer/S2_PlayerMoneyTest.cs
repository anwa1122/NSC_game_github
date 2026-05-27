using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class S2_PlayerMoneyTest : MonoBehaviour
{
    public static S2_PlayerMoneyTest Instance;
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

    public void SubtractMoney(float getMoney)
    {
        playerMoney -= getMoney;
        playerMoneytext.text = "PlayerMoney : " + playerMoney;
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestItemSlot : MonoBehaviour
{
    public TextMeshProUGUI titleText;//บอกตำแหน่งว่า title image frame อยุ่ไหน
    public Image iconImage;
    public Image frameImage;

    private bool isRainbow = false; //ตัวแปรเอาไว้บอกว่า เป็น rainbow ยัง
    private bool isRare = false;
    [HideInInspector]
    public QuestData currentData; //ตัวแปร currenData โดยจะกำหนดใช้ในสคริปต์อื่นจะมีค่าข้มูลเป็นสคริปต์ของ QuestData

    public void Setup(QuestData data, bool isRareThisTime) //ฟังชัน Setup เอาไว้ setup ข้อมูลต่างๆ โดยจะเรียกใช้ ข้อมูลในสคริปต์ QuestData และเอาข้อมูลว่ามันแรร์มั้ย
    {
        currentData = data; //ให้ currentData มีค่าข้อมูลเป็น data ที่ถูกโอนมาจากการเรียกใ้ที่สคริปต์อื่น
        titleText.text = data.questName;
        iconImage.sprite = data.questIcon;

        isRainbow = isRareThisTime; //ถ้ามันแรร์ก้ให้เป็น rainbow
        isRare = isRareThisTime;

        if (frameImage != null) //ถ้ามี frameImage
        {
            frameImage.gameObject.SetActive(isRareThisTime); //ให้มันแสดง frame ออกมาเพื่อให้ผู้เล่นได้เห็นสี rainbow
        }
    }

    void Update()
    {
        if (isRainbow && frameImage != null) //ถ้า rainbow เป็นจริง ให้มันเล่นสีเรนโบว์
        {
            float speed = 0.25f; //ความเร็ว
            frameImage.color = Color.HSVToRGB(Mathf.PingPong(Time.time * speed, 1), 1, 1); //การเล่นสีรุ้ง
        }
    }

    public void OnClick() //ผู้เฃ่นคลิกเอาไว้ใช้ในอนาคตตอนนี้ยังไมไ่ด้ใช้
    {
        QuestDetailPanel.Instance.DisplayQuest(currentData, isRare);
    }
}

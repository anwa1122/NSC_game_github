using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestItemSlot : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public Image iconImage;
    public Image frameImage;

    private bool isRainbow = false;
    private QuestData currentData;

    

    public void Setup(QuestData data, bool isRareThisTime)
    {
        currentData = data;
        titleText.text = data.questName;
        iconImage.sprite = data.questIcon;

        isRainbow = isRareThisTime;

        if(frameImage != null)
        {
            frameImage.gameObject.SetActive(isRareThisTime);
        }
    }

    void Update()
    {
        if (isRainbow && frameImage != null)
        {
            float speed = 0.25f;
            frameImage.color = Color.HSVToRGB(Mathf.PingPong(Time.time * speed, 1), 1, 1);
        }
    }

    public void OnClick()
    {
        Debug.Log("Player choose" + currentData.questName);
    }
}

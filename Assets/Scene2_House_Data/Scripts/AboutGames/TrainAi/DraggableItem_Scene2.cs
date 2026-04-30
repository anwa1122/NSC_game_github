using UnityEngine;
using UnityEngine.UI;
public class DraggableItem_Scene2 : MonoBehaviour
{
    public ItemData_Scene2 itemData;
    private Image displayImage;


    public void SetupItem(ItemData_Scene2 data)
    {
        displayImage = GetComponent<Image>();
        itemData = data;

        if (itemData.icon != null)
        {
            displayImage.sprite = itemData.icon;
        }
        else
        {
            displayImage.color = (itemData.category == ItemCategory_Scene2.Trash) ? Color.gray : Color.yellow;
        }
    }
}

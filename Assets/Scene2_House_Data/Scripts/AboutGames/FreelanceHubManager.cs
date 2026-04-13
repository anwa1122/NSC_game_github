using UnityEngine;
using System.Collections.Generic; 
public class FreelanceHubManager : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform contentParent;
    public List<QuestData> allQuests;
    void Start()
    {
        GenerateQuestList();
    }

    public void GenerateQuestList()
    {
        foreach (QuestData data in allQuests)
        {
            GameObject newSlot = Instantiate(slotPrefab, contentParent);

            QuestItemSlot slotScript = newSlot.GetComponent<QuestItemSlot>();

            bool isRare = Random.Range(0f, 100f) <= 20f;

            slotScript.Setup(data, isRare);
        }
    }
}

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TrashMiniGameController : MonoBehaviour, IResettable
{
    [Header("Ui")]
    public Button confirmButton;
    public GameObject sortingGamePanel;
    public GameObject trashPrefab;

    [Header("CanvasGroup")]
    public CanvasGroup canvasGroup;//e
    public float fadeSpeed = 2f;
    public TextMeshProUGUI text;
    public float showDuration = 2f;

    [Header("Object")]
    public RectTransform spawnArea;
    public Transform ItemFather;
    public bool winTheGame = false;

    private bool firstTime = true;
    private float playerMoney;

    private List<GameObject> spawnedTrashes = new List<GameObject>();

    // --- ระบบ Reset สำหรับวันใหม่ ---
    public void ResetObject()
    {
        winTheGame = false;
        firstTime = true;

        if (sortingGamePanel != null) sortingGamePanel.SetActive(false);

        foreach (Transform child in ItemFather)
        {
            if (child != null) Destroy(child.gameObject);
        }

        spawnedTrashes.Clear();
        if (confirmButton != null) confirmButton.interactable = false;

        Debug.Log("MiniGame Controller Reset!");
    }

    public void SetUpMiniGame()
    {
        // ปลดล็อกเมาส์ให้พร้อมเล่น
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        confirmButton.interactable = false;

        if (firstTime == true)
        {
            foreach (TrashData data in InventoryManager.Instance.items)
            {
                GameObject obj = Instantiate(trashPrefab, ItemFather);
                DraggableTrash script = obj.GetComponent<DraggableTrash>();
                script.data = data;
                obj.GetComponent<Image>().sprite = data.icon;
                obj.transform.localPosition = GetRandomPosInArea();
                spawnedTrashes.Add(obj);
            }
            firstTime = false;
        }
        else
        {
            foreach (Transform child in ItemFather)
            {
                child.transform.localPosition = GetRandomPosInArea();
            }
        }
    }

    public void ConfirmSelection()
    {
        float allMoney = 0;
        foreach (Transform child in ItemFather)
        {
            DraggableTrash dragTrashScript = child.GetComponent<DraggableTrash>();
            InventoryManager.Instance.RemoveItem(dragTrashScript.data);

            if (dragTrashScript.currentSlot == dragTrashScript.data.type)
            {
                PlayerDataManager.Instance.AddMoney(dragTrashScript.data.scoreValue);
                allMoney += dragTrashScript.data.scoreValue;
            }
        }
        confirmButton.interactable = false;
        winTheGame = true;

        text.text = "Done Sorting!! Earn " + allMoney + " Moneys";
        StartCoroutine(TimedTextRoutine(showDuration));

        Invoke("exitTheGame", showDuration);
    }

    public void exitTheGame()
    {
        sortingGamePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UpdateConfirmButtonState()
    {
        bool allPlaced = true;
        int childCount = 0;
        foreach (Transform child in ItemFather)
        {
            childCount++;
            DraggableTrash script = child.GetComponent<DraggableTrash>();
            if (script.currentSlot == TrashType.None)
            {
                allPlaced = false;
                break;
            }
        }
        confirmButton.interactable = (childCount > 0 && allPlaced);
    }

    private Vector3 GetRandomPosInArea()
    {
        float x = Random.Range(-spawnArea.rect.width / 2.5f, spawnArea.rect.width / 2.5f);
        float y = Random.Range(-spawnArea.rect.height / 2.5f, spawnArea.rect.height / 2.5f);
        return new Vector3(x, y, 0);
    }

    IEnumerator FadeUI(bool fadeIn)
    {
        float targetAlpha = fadeIn ? 1f : 0f; // ถ้า fadeIn เป็น true เป้าหมายคือ 1 (เข้ม)

        if (fadeIn) canvasGroup.gameObject.SetActive(true);

        // วนลูปจนกว่า Alpha จะใกล้เคียงเป้าหมาย
        while (!Mathf.Approximately(canvasGroup.alpha, targetAlpha))
        {
            // ค่อยๆ ปรับ Alpha ไปหาเป้าหมายตามเวลาจริง
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
            yield return null; // รอเฟรมถัดไป
        }

        if (!fadeIn) canvasGroup.gameObject.SetActive(false);
    }

    private IEnumerator TimedTextRoutine(float duration)
    {
        // 1. ค่อยๆ ปรากฏขึ้นมา
        yield return StartCoroutine(FadeUI(true));

        // 2. รอตามเวลาที่กำหนด
        yield return new WaitForSeconds(duration);

        // 3. ค่อยๆ จางหายไป
        yield return StartCoroutine(FadeUI(false));
    }
}
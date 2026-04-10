using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // สำหรับการเปลี่ยนฉาก
using System.Collections;         // สำหรับใช้ Coroutine

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance; // ตัวแปรส่วนกลางที่เรียกใช้ได้จากทุกที่

    [Header("UI Display")]
    public TextMeshProUGUI playerMoneyText;

    [Header("Player Data")]
    public float money; // เก็บค่าเงิน

    [Header("Transition Settings")]
    public CanvasGroup transitionCG; // ลาก TransitionCanvas มาใส่
    public float fadeDuration = 1f;  // ความเร็วในการจาง

    private void Awake()
    {
        // ระบบ Singleton: ตรวจสอบว่ามีตัวซ้ำไหม ถ้าไม่มีให้คงอยู่ตลอดไป
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        // เช็คก่อนว่ามี UI ให้แสดงผลไหม ถ้าไม่มี (เช่นตอนเปลี่ยนฉาก) จะได้ไม่ Error
        if (playerMoneyText != null)
        {
            playerMoneyText.text = "totalMoney : " + money; 
        }
    }

    public void AddMoney(float amount)
    {
        money += amount;
        // ปรับ UI ทันทีเมื่อเงินเพิ่ม
        if (playerMoneyText != null)
        {
            playerMoneyText.text = "totalMoney : " + money; 
        }
    }

    // --- ระบบวาร์ปข้าม Scene ---

    public void MoveToScene(string sceneName)
    {
        StartCoroutine(TransitionRoutine(sceneName));
    }

    IEnumerator TransitionRoutine(string sceneName)
    {
        // 1. ค่อยๆ มืด (Fade Out)
        float timer = 0;
        if (transitionCG != null)
        {
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                transitionCG.alpha = timer / fadeDuration;
                yield return null;
            }
            transitionCG.alpha = 1;
        }

        // 2. โหลด Scene ใหม่ (รอจนกว่าจะโหลดเสร็จ)
        yield return SceneManager.LoadSceneAsync(sceneName);

        // 3. ค่อยๆ สว่าง (Fade In) ใน Scene ใหม่
        timer = fadeDuration;
        if (transitionCG != null)
        {
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                transitionCG.alpha = timer / fadeDuration;
                yield return null;
            }
            transitionCG.alpha = 0;
        }
    }

    // ฟังก์ชันสำหรับให้ UI ในฉากใหม่ส่งตัวเองมาเชื่อมต่อ (ใช้ในขั้นตอนถัดไป)
    public void UpdateMoneyTextReference(TextMeshProUGUI newText)
    {
        playerMoneyText = newText;
    }
}
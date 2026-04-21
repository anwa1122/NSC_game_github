using TMPro;
using UnityEngine;

public class RandomWord : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    public void Setup(string word)
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = word;
    }

    // ฟังก์ชันให้ตัวแม่สั่งทำลาย
    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
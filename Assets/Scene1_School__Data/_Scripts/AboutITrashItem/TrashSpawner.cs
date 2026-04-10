using System.Collections.Generic;
using UnityEngine;

// 1. เพิ่ม , IResettable เพื่อบอกว่าสคริปต์นี้รีเซ็ตได้
public class TrashSpawner : MonoBehaviour, IResettable
{
    [Header("คลังขยะ")]
    public List<TrashData> commonTrash;
    public List<TrashData> rareTrash;

    [Header("จุดเกิด")]
    public Transform[] clusterNodes;
    public Transform[] singleNodes;

    [Header("ตั้งค่าการสุ่ม")]
    public GameObject basePrefab;
    [Range(0, 100)] public float spawnChance = 70f;
    [Range(0, 100)] public float mixChance = 20f;

    [Header("ระบบโควตา (กันซ้ำ)")]
    public int maxDuplicates = 3;
    private Dictionary<TrashData, int> spawnTracker = new Dictionary<TrashData, int>();

    // 2. เพิ่ม List เพื่อจำว่ามีขยะชิ้นไหนอยู่ในฉากบ้าง
    private List<GameObject> activeTrash = new List<GameObject>();

    void Start() { SpawnAll(); }

    // 3. ฟังก์ชันรีเซ็ตตามกฎของ Interface
    public void ResetObject()
    {
        Debug.Log("Cleaning old trash and spawning new ones...");
        
        // ลบขยะเก่าที่ยังค้างอยู่ในฉากทิ้งให้หมด
        foreach (GameObject trash in activeTrash)
        {
            if (trash != null) Destroy(trash);
        }
        activeTrash.Clear(); // ล้าง List ให้ว่าง

        // สั่งเสกขยะชุดใหม่
        SpawnAll();
    }

    void SpawnAll()
    {
        spawnTracker.Clear();

        foreach (Transform node in clusterNodes)
        {
            if (Random.Range(0, 100) > spawnChance) continue;
            TrashData mainTrash = GetTrashWithQuota(commonTrash);

            foreach (Transform spawnPoint in node)
            {
                if (Random.Range(0, 100) > 80) continue;
                TrashData trashToSpawn = (Random.Range(0, 100) < mixChance)
                    ? GetTrashWithQuota(commonTrash)
                    : mainTrash;

                CreateTrash(trashToSpawn, spawnPoint);
            }
        }

        foreach (Transform point in singleNodes)
        {
            if (Random.Range(0, 100) > 50) continue;
            TrashData selected = (Random.Range(0, 100) < 10)
                ? GetTrashWithQuota(rareTrash)
                : GetTrashWithQuota(commonTrash);

            CreateTrash(selected, point);
        }
    }

    TrashData GetTrashWithQuota(List<TrashData> pool)
    {
        if (pool.Count == 0) return null;
        List<TrashData> validOptions = new List<TrashData>();
        foreach (var trash in pool)
        {
            if (!spawnTracker.ContainsKey(trash) || spawnTracker[trash] < maxDuplicates)
            {
                validOptions.Add(trash);
            }
        }
        TrashData selected = (validOptions.Count > 0)
            ? validOptions[Random.Range(0, validOptions.Count)]
            : pool[Random.Range(0, pool.Count)];

        if (spawnTracker.ContainsKey(selected)) spawnTracker[selected]++;
        else spawnTracker.Add(selected, 1);

        return selected;
    }

    void CreateTrash(TrashData data, Transform pos)
    {
        if (data == null) return;
        float randomY = Random.Range(0f, 360f);
        float randomX = Random.Range(-5f, 5f); 
        Quaternion randomRot = Quaternion.Euler(randomX, randomY, pos.rotation.eulerAngles.z);

        GameObject obj = Instantiate(basePrefab, pos.position, randomRot);
        
        // 4. เก็บขยะที่สร้างใหม่ลงใน List เพื่อให้ตามไปลบได้ถูกตัว
        activeTrash.Add(obj);
        
        obj.GetComponent<CollectibleTrash>().Setup(data);
    }
}
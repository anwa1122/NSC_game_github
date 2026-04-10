using UnityEngine;

public class PlayerHitbox_Scene2 : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("SitHitbox"))
        {
            Debug.Log("Stayed");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SitHitbox"))
        {
            
        }
    }
}

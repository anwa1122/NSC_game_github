using UnityEngine;

public class RedSlotScript : MonoBehaviour
{
    public GameObject target;
    public bool thiswin = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == target.name)
        {
            thiswin = true;
        }
    }
}

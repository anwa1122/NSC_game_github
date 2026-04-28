using UnityEngine;
using System.Collections.Generic;
public class SlotManager : MonoBehaviour
{
    public List<GameObject> allObj;
    public bool redWin;
    public bool blueWin;
    public bool yellowWin;
    public bool greenWin;
    void Update()
    {
        winAll();
        if (redWin && blueWin && yellowWin && greenWin)
        {
            Debug.Log("You win");
        }
    }
    void winAll()
    {
        foreach (GameObject target in allObj)
        {
            if (target.GetComponent<RedSlotScript>())
            {
                redWin = target.GetComponent<RedSlotScript>().thiswin;
            }
            else if (target.GetComponent<BlueSlotScript>())
            {
                blueWin = target.GetComponent<BlueSlotScript>().thiswin;
            }
            else if (target.GetComponent<YellowSlotScript>())
            {
                yellowWin = target.GetComponent<YellowSlotScript>().thiswin;
            }
            else if (target.GetComponent<GreenSlotScript>())
            {
                greenWin = target.GetComponent<GreenSlotScript>().thiswin;
            }
        }
    }
}

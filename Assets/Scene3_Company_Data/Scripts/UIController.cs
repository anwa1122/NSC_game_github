using UnityEngine;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using System.Linq;

public class UIController_Scene3 : MonoBehaviour
{
    public static UIController_Scene3 Instance;

    public GameObject firstGameObjInList;
    public GameObject lastGameObj;
    public List<GameObject> windowList;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void LaunchInterface(GameObject target)
    {
        if (lastGameObj != null)
        {
            lastGameObj.SetActive(false);
        }
        else
        {
            lastGameObj = target;
        }
        windowList.Add(target);
        firstGameObjInList = windowList[windowList.Count - 1];
        firstGameObjInList.SetActive(true);
    }

    public void ExitInterface()
    {
        windowList.RemoveAt(windowList.Count - 1);
        firstGameObjInList.SetActive(false);
        if (windowList.Count > 0)
        {
            firstGameObjInList = windowList[windowList.Count - 1];
            firstGameObjInList.SetActive(true);
        }

    }

}

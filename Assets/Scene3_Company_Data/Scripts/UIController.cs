using UnityEngine;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using System.Linq;

public class UIController_Scene3 : MonoBehaviour
{
    public static UIController_Scene3 Instance;

    private GameObject firstGameObj;
    public List<GameObject> windowList;
    void Awake()
    {
        if(Instance == null) Instance = this;
    }

    public void LaunchInterface(GameObject target)
    {
        windowList.Add(target);
        target.SetActive(true);
    }

    public void ExitInterface(GameObject target)
    {
        windowList.RemoveAt(windowList.Count - 1);
        target.SetActive(false);
        
        if (windowList.Count > 0)
        {
            firstGameObj = windowList[windowList.Count - 1];
            firstGameObj.SetActive(true);
        }
        
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputActions{Inventory,UseConsumeable,Reload, CancelShot};
public class Temp_KeyMapper : MonoBehaviour
{
    static Dictionary<InputActions, KeyCode> dict;

    [Header("Version 1")]
    [Tooltip("Inventory,UseConsumeable,Reload - 0,1,2")]
    public KeyCode[] keyCodes_v1; 
    [Header("Version 2")]
    [Tooltip("Inventory,UseConsumeable,Reload - 0,1,2")]
    public KeyCode[] keyCodes_v2;

    public static int currentVersion = 0;

    public static KeyCode[][] keyCode_versions;

    public RuntimeAnimatorController semiAutoBowAnimator;
    public RuntimeAnimatorController autoBowAnimator;

    public Bow[] bows;

    [SerializeField]
    UnityEngine.UI.Toggle isHoldToggle;
    
    
    [SerializeField]
    UnityEngine.UI.Toggle isCrosshair;
    [SerializeField]
    UnityEngine.UI.Image crosshair;
    
    
    /// <summary>
    /// True means Toggle - False means Hold
    /// </summary>
    public static bool ToggleOrHold => currentVersion == 1;
    private void Awake()
    {
        dict = new Dictionary<InputActions, KeyCode>();
        keyCode_versions = new KeyCode[2][];
        keyCode_versions[0] = keyCodes_v1;
        keyCode_versions[1] = keyCodes_v2;

        SetInputVersion(currentVersion);

        //semiAutoBowAnimator.
    }


    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Backspace))
    //    {
    //        if (currentVersion == 0)
    //            SetInputVersion(1);
    //        else
    //            SetInputVersion(0);
    //    }
    //}

    /// <summary>
    /// 0 - version1 ; 1 - version2
    /// </summary>
    /// <param name="v"></param>
    public void SetInputVersion(int v)
    {
        currentVersion = v;
        SetKeys(keyCode_versions[currentVersion]);
        foreach (var item in bows)
        {
            item.SetAnimatorController( v == 0 ? autoBowAnimator : semiAutoBowAnimator);
        }
    }

    void SetKeys(KeyCode[] keys)
    {
        dict.Clear();
        for (int i = 0; i < keys.Length; i++)
        {
            dict.Add((InputActions)i, keys[i]);
        }
    }

    public static KeyCode GetKeycodeForInputAction(InputActions inputAction)
    {
        return dict[inputAction];
    }

    public void VersionToggle()
    {
        SetInputVersion(isHoldToggle.isOn ? 1 : 0);

        if(PlayerController.CurrentBow.gameObject.activeInHierarchy) //should check for seperate Semi/Auto Reload boolean TBD   
        {
            PlayerController.CurrentBow.KillLoadedArrow();
        }
    }

    public void CrosshairToggle()
    {
        crosshair.gameObject.SetActive(isCrosshair.isOn);
    }
}

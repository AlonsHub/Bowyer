using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputActions{Inventory,UseConsumeable,Reload};
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
    }

    //Even more temp than this whole script
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            if (currentVersion == 0)
                SetInputVersion(1);
            else
                SetInputVersion(0);
        }
    }

    /// <summary>
    /// 0 - version1 ; 1 - version2
    /// </summary>
    /// <param name="v"></param>
    public void SetInputVersion(int v)
    {
        currentVersion = v;
        SetKeys(keyCode_versions[currentVersion]);
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
}

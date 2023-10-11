using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedsAndSensitivities : MonoBehaviour
{
    static SpeedsAndSensitivities Instance;


    [SerializeField]
    Vector2 _baseLookSpeeds;//Should be 90f,90f

    Vector2 _lookSpeeds => _baseLookSpeeds - (_currentBowWeight * Vector2.one + _currentPullWeight * Vector2.one); 
    public static float GetLookSpeed(AxisDirection axisDirection) => (axisDirection == AxisDirection.X) ? Instance._lookSpeeds.x : Instance._lookSpeeds.y;

    public static float BaseCameraFOV = 60f; 

    float _currentBowWeight; // 0f if no bow
    float _currentPullWeight; // 0f if no pull
    
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SetBowWeightToDefault();
    }



    public static void SetBowWeight(float bowWeight)
    {
        Instance._currentBowWeight = bowWeight;
        Instance._currentPullWeight = 0f;

       
    }

    public static void SetPullWeight(float pullWeight)
    {
        Instance._currentPullWeight = pullWeight;
    }

    public static void SetBowWeightToDefault()
    {
        Instance._currentBowWeight = 0f;
    }

}

//public class MouseInputSettings
//{
//    public float gravity;
//    public float sensitivity;
//    public float dead;

//    public MouseInputSettings(float g, float s, float d)
//    {
//        gravity = g;
//        sensitivity = s;
//        dead = d;
//    }
//}

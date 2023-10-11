using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedsAndSensitivities : MonoBehaviour
{
    static SpeedsAndSensitivities Instance;


    [SerializeField]
    Vector2 _baseLookSpeeds;//Should be 90f,90f

    Vector2 _lookSpeeds; 
    public static float GetLookSpeed(AxisDirection axisDirection) => (axisDirection == AxisDirection.X) ? Instance._lookSpeeds.x : Instance._lookSpeeds.y;

    float _currentBowWeight; // -1f if no bow
    
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

        Instance._lookSpeeds = Instance._baseLookSpeeds - bowWeight * Vector2.one;
    }
    public static void SetBowWeightToDefault()
    {
        Instance._currentBowWeight = -1f;
        Instance._lookSpeeds = Instance._baseLookSpeeds;
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

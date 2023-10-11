using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpeedsAndSensitivities 
{
    //Settings Stats: (user)

    //Base Stats: (gamedesign)
    static MouseInputSettings defaultMouseInputSettings => new MouseInputSettings(0f,.1f, 0.01f);
    public static MouseInputSettings CurrentMouseInputSettings;
    //Affectors?

    public static void SetBowWeight(float bowWeight)
    {
        CurrentMouseInputSettings = new MouseInputSettings(25f/bowWeight,25f/bowWeight, 0.01f);
    }
    public static void SetBowWeightToDefault()
    {
        CurrentMouseInputSettings = defaultMouseInputSettings;
    }

}

public class MouseInputSettings
{
    public float gravity;
    public float sensitivity;
    public float dead;

    public MouseInputSettings(float g, float s, float d)
    {
        gravity = g;
        sensitivity = s;
        dead = d;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixedJoystick : Joystick
{
    public void EnableVisisbility(Image val)
    {
        Color color = val.color;

        color.a = 1f;
        val.color = color;
    }
    
    public void DisableVisisbility(Image val)
    {
        Color color = val.color;

        color.a = 0f;
        val.color = color;
    }
    
}
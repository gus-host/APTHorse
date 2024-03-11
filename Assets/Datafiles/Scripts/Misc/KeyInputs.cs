using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputs : MonoBehaviour
{
    public static KeyInputs instance;
    
    private void Start()
    {
        instance = this;
    }

    public KeyCode _breakWall = KeyCode.B;
}

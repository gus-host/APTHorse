using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public static Sword instance;

    public int damageAmount = 20;

    [Header("Bools")]
    public bool _canBreak = false;

    void Start()
    {
        instance = this;
        PlayerUI.instance._wallBreakbtn.onClick.AddListener(BreakWall);
    }

    // Update is called once per frame
    void Update()
    {
        if (_canBreak)
        {
            if (Input.GetKeyDown(KeyInputs.instance._breakWall)) // Assuming left mouse button is used for sword attack
            {
                BreakWall();
            }
        }
    }

    private void BreakWall()
    {
       SmashableWall._instance._canSmash = true;
    }
}

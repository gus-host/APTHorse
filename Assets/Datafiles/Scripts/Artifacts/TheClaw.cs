using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheClaw : MonoBehaviour
{
    public static TheClaw instance;
    public GameObject _freeze;
    public GameObject _darkMagic;
    public GameObject _energyConsume;

    private void Start()
    {
        instance = this;
    }
}

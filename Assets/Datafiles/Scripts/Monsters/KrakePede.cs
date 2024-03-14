using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakePede : MonoBehaviour
{
    public static KrakePede Instance;

    private void Start()
    {
        Instance = this;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    public GameObject[] EnvironmentLight;
    void Start()
    {
        foreach(GameObject light in EnvironmentLight)
        {
            light.SetActive(false);
        }
    }

  
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntrantMainMenuManager : MonoBehaviour
{
    [SerializeField]private TMP_Dropdown _dropdown;

    private void Start()
    {
        _dropdown.value = Mathf.FloorToInt(_dropdown.options.Count/2);
    }

    public void OnQualityChanged()
    {
        QualitySettings.SetQualityLevel(_dropdown.value);
    }
    
}

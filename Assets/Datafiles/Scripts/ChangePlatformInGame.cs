using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangePlatformInGame : MonoBehaviour
{
    public TMP_Dropdown _changePlatformDropdown;

    public void OnPlatformChanged()
    {
        GameManager.instance._currentPlayerRef.GetComponent<IntrantPlayerInput>().ChangePlatform(_changePlatformDropdown.value);
    }
    
}

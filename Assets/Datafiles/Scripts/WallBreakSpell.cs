using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBreakSpell : MonoBehaviour
{
    public GameObject _pointer;

    private void OnDestroy()
    {
        Destroy(_pointer);
    }
}

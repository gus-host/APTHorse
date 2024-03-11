using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentOne : MonoBehaviour
{

    private int _childCount = 0;

    public int childCount
    {
        get
        {
            return _childCount;
        }
        set
        {
            _childCount = value;
        }
    }
    
}

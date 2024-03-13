using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HorseSer
{
    H1,
    H2,
    H3,
    H4,
    H5
}


public class WayPointList : MonoBehaviour
{
    public HorseSer horseSer;

   public Transform []_wayPoint;

    int i = 1;
    private void Start()
    {
        foreach (Transform go in _wayPoint)
        {
            go.name = "Waypoint " + i++;
        }
    }
}

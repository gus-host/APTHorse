using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointList : MonoBehaviour
{
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

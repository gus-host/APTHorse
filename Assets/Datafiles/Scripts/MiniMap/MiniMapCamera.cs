using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    public Transform target;
           public float height = 100;
           public bool followPosition; //should the camera rotate with the target
           public bool followRotation; //should the camera rotate with the target
   
           void Update()
           {
               try
               {
                   if (!target)
                       target = FindObjectOfType<IntrantThirdPersonController>().transform ;
               }
               catch (Exception e)
               {
               }
           }
   
           void LateUpdate()
           {
               if (!target) return;
   
               if (followRotation)
                   transform.eulerAngles = new Vector3(transform.eulerAngles.x, target.eulerAngles.y, transform.eulerAngles.z);
   
               if (followPosition)
                   transform.position = new Vector3(target.position.x, height, target.position.z);
           }
}

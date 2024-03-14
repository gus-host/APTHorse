using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
   private Transform mainCameratransform;

   private void Start()
   {
      mainCameratransform = Camera.main.transform;
   }

   private void LateUpdate()
   {
      transform.LookAt(transform.position + mainCameratransform.rotation * Vector3.forward, 
         mainCameratransform.rotation * Vector3.up);
   }
}

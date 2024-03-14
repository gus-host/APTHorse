using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveThings : MonoBehaviour
{
   public void MoveUpAndDown(float val, float interval)
    {
        Vector3 _dir = new Vector3(transform.position.x, transform.position.y+val, transform.position.z);
        LeanTween.moveLocal(gameObject, _dir, interval);
    }
}

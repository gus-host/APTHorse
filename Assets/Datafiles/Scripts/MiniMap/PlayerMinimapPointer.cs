using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMinimapPointer : MonoBehaviour
{
    public Transform target;
    public float height = 25.0f;
    private float wantedHeight;
    public int markerSize = 25;
    public bool targetFound = false;
    private void Awake()
    {
        this.transform.localScale = new Vector3(markerSize, 0.1f, markerSize);
    }
    void Start()
    {
        if (target != null)
        {
            wantedHeight = target.position.y + height;
        }
    }

    void Update()
    {
        if (!target)
        {
            if(!targetFound)
            {
                return;
            }else if(targetFound)
            {
                Destroy(gameObject);
            }
        }
        else if(target !=null)
        {
            targetFound = true;
            //follow the racer
            transform.position = new Vector3(target.position.x, wantedHeight, target.position.z);

            //Rotate in the direction of the racer
            Quaternion rot = transform.rotation;
            rot = target.rotation;
            rot.x = 0;
            rot.z = 0;
            transform.rotation = rot;
        }

    }
}

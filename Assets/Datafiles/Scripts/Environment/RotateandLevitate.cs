using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateandLevitate : MonoBehaviour
{

    [SerializeField] public Vector3 _speed;
    public float levitationHeight = 1f;
    public float levitationSpeed = 1f;
    private Vector3 startingPosition;


    public bool rotate = true;
    public bool levitate = true;
    
    private void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        if (rotate)
        {
            Quaternion deltaRotation = Quaternion.Euler(_speed * Time.deltaTime);
            transform.rotation *= deltaRotation;
        }
        


        //levitate
        if (levitate)
        {
            float yPos = Mathf.PingPong(Time.time * levitationSpeed, levitationHeight);
            Vector3 newPosition = startingPosition + new Vector3(0f, yPos, 0f);
            transform.position = newPosition;
        }
    }
}

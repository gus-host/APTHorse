using System;
using System.Collections.Generic;
using UnityEngine;

public class FrontRadar : MonoBehaviour
{
    public bool stop = false;
    public LayerMask layer;

    public static Action<bool> _stopBroadcast;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == layer)
        {
            Debug.Log("Laye matched ");
            if (other.gameObject != gameObject)
            {
                stop = true;
                _stopBroadcast?.Invoke(stop);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == layer)
        {
            Debug.Log("Laye matched ");
            if(other.gameObject != gameObject)
            {
                stop = false;
                _stopBroadcast?.Invoke(stop);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == layer)
        {
            Debug.Log("Laye matched ");
            if (collision.gameObject != gameObject)
            {
                stop = true;
                _stopBroadcast?.Invoke(stop);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == layer)
        {
            Debug.Log("Laye matched ");
            if (collision.gameObject != gameObject)
            {
                stop = false;
                _stopBroadcast?.Invoke(stop);
            }
        }
    }
}

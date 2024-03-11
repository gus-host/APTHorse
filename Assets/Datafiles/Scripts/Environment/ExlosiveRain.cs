using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExlosiveRain : MonoBehaviour
{
    public GameObject rainItem;

    public GameObject[] spts;
    public bool invoked = false;
    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.PLAYER_TAG))
        {
            if (!invoked)
            {
                InvokeRepeating("SpawnRep",0, 2f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CancelInvoke("SpawnRep");
        invoked = false;
    }

    public void SpawnRep()
    {
        invoked = true;
        GameObject arrow = Instantiate(rainItem, spts[Random.Range(0, spts.Length)].transform.position,Quaternion.identity);
    }
}

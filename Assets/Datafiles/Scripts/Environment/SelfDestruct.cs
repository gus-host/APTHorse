using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public GameObject selfDestructObjectPrefab;

    public bool selfDestructObject;

    public bool selfDestructObjectFX;

    // Start is called before the first frame update
    void Start()
    {
        if(selfDestructObjectFX)
        {
            Destroy(gameObject, 5f);   
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (selfDestructObject && other.gameObject.CompareTag(Tags.PLAYER_TAG))
        {
            selfDestructObjectPrefab.SetActive(true);
        }
    }

}

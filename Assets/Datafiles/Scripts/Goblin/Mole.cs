using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Goblin" || other.gameObject.GetComponent<GoblinManager>())
        {
            Debug.LogError("Deleting goblin");
            Destroy(other.gameObject, 0.5f);
        }
    }
}

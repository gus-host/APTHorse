using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalTeleport : MonoBehaviour
{
    public Transform _teleportFrom;
    public Transform _teleportTo;


    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
           StartCoroutine(Teleport(other, rb));
        }
    }

    private IEnumerator Teleport(Collider other, Rigidbody rb)
    {
        rb.isKinematic = true;
        yield return new WaitForSeconds(0.1f);
        other.transform.position = _teleportTo.position;
        yield return new WaitForSeconds(0.1f);
        rb.isKinematic = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

public class PvPHammer : MonoBehaviour
{
    [SerializeField] public int damageToDeal = 20;

    private Collider _hammerCollider;

    private void Start()
    {
        _hammerCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PvPPlayerHealth>(out PvPPlayerHealth health))
        {
            if (!other.GetComponent<PhotonView>().IsMine)
            {
                health.DealDamage(damageToDeal);
                Debug.LogError("Giving Damage");
            }
        }
    }

    // If you want to enable the collider on non-host systems as well
    public void EnableCollider(bool enabled)
    {
        EnableColliderServer(enabled);
    }
    
    public void EnableColliderServer(bool enabled)
    {
        _hammerCollider.enabled = enabled;
    }
}

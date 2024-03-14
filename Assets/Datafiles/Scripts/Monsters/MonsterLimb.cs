using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MonsterLimb : MonoBehaviour
{
    [SerializeField] public float damageToDeal;
    public bool _ishands;
    
    private void Start()
    {
        GetComponent<BoxCollider>().isTrigger = true;
        damageToDeal = 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IntrantPlayerHealthManager>(out IntrantPlayerHealthManager health))
        {
            health.DealDamage(damageToDeal);
            Debug.Log("Giving Damage to player");
        }
    }
}

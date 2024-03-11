using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamageOnExplode : MonoBehaviour
{
    private float minDamage = 0.1f;
    private float maxDamage = 0.9f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IntrantPlayerHealthManager>(out IntrantPlayerHealthManager health))
        {
            health.DealDamage(Random.Range(minDamage, maxDamage));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField] public int damageToDeal = 20;
    public LayerMask layerToCheck;
    private void OnTriggerEnter(Collider other)
    {
/*        if (layerToCheck == (layerToCheck | (1 << other.gameObject.layer)))
        {
            Debug.LogError("ignoring attack");
            GetComponent<BoxCollider>().enabled = false;
            return;
            // Do something when colliding with an object on the specified layer
        }*/
        if (other.TryGetComponent<MonsterHealthController>(out MonsterHealthController health))
        {
            health.GiveDamage(damageToDeal);
        }

        if (other.TryGetComponent<GoblinHealthManager>(out GoblinHealthManager _health))
        {
            _health.ApplyDamage(damageToDeal);
        }
        
        if (other.TryGetComponent<HealthScript>(out HealthScript _kairehealth))
        {
            _kairehealth.ApplyDamage(damageToDeal);
        }
    }
    private void OnTriggerExit(Collider other)
    {
/*        if (layerToCheck == (layerToCheck | (1 << other.gameObject.layer)))
        {
            Debug.LogError("ignoring attack");
            GetComponent<BoxCollider>().enabled = true;
            return;
            // Do something when colliding with an object on the specified layer
        }*/
    }
}

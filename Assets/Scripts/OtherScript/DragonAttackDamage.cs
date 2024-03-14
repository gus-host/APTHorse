using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttackDamage : MonoBehaviour
{
    public LayerMask layer;
    public float radius = .3f;
    public float damage = 1f;

   
    // Update is called once per frame
    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layer);

        if (hits.Length > 0 && (hits[0].GetComponent<DragonHealth_Script>()))
        {
            print("hits[0]" + hits[0]);
            hits[0].GetComponent<DragonHealth_Script>().ApplyDamage(damage);
            print("gameObject"+gameObject.name);
            gameObject.SetActive(false);

        }
    }
}

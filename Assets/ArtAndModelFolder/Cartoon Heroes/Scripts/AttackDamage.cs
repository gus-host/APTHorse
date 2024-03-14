using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackDamage : MonoBehaviour
{
    public LayerMask _layer;
    public List<int> _ignoreMask = new List<int>();
    public float radius = .5f;
    public float _minDamage = 1f;
    public float _maxDamage = 1f;
    
    public GameObject PlayerSound;
    
    [SerializeField]
    private float timer = 0f;

    [SerializeField] Collider[] hits;
    
    private void Awake()
    {
        Debug.LogWarning("Selected Layer to be damaged "+ _ignoreMask.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        hits = Physics.OverlapSphere(transform.position, radius, _layer);
        if(hits.Length > 0 )
        {
            foreach(var hit in hits)
            {
                Debug.LogWarning("Player Hit "+ hit);
            }
        }
        CheckForPlayerHit();
        //CheckForDragonHit();
        //CheckForGoblinHit();
        Array.Clear(hits, 0, hits.Length);

    }

    private void CheckForGoblinHit()
    {
        float damage = Mathf.Ceil(UnityEngine.Random.Range(_minDamage, _maxDamage));

        foreach (int layer in _ignoreMask)
        {
            if (hits.Length > 0 && (hits[0].GetComponent<GoblinHealthManager>()))
            {
                if (hits[0].gameObject.layer == layer)
                {
                    hits[0].GetComponent<GoblinHealthManager>().ApplyDamage((int)damage);

                    GameManager.instance.sfx.Attack_1();

                    gameObject.SetActive(false);
                }
            }
        }
    }

    private void CheckForDragonHit()
    {
        float damage = Mathf.Ceil(UnityEngine.Random.Range(_minDamage, _maxDamage));

        if (hits.Length > 0 && (hits[0].GetComponent<DragonHealth_Script>()))
        {
            hits[0].GetComponent<DragonHealth_Script>().ApplyDamage(damage);

            GameManager.instance.sfx.Attack_1();

            gameObject.SetActive(false);
        }
    }

    private void CheckForPlayerHit()
    {
        float damage = Mathf.Ceil(UnityEngine.Random.Range(_minDamage, _maxDamage));
        foreach (int layer in _ignoreMask)
        {
            if (hits.Length > 0 && (hits[0].GetComponent<IntrantPlayerHealthManager>()))
            {
                if (hits[0].gameObject.layer == layer)
                {
                    hits[0].GetComponent<IntrantPlayerHealthManager>().DealDamage(Mathf.RoundToInt(Mathf.Ceil(damage)));

                    GameManager.instance.sfx.Attack_1();

                    gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, radius);

    }
}

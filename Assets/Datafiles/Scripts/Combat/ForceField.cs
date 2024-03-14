using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceField : MonoBehaviour
{
    public GameObject FreezeVFX;
    public bool Active = false;
    List<MonsterMovementController> _monsters = new List<MonsterMovementController>();


    private void LateUpdate()
    {
        if (!Active && _monsters.Count > 0)
        {
            foreach(var monster in _monsters)
            {
                if (monster != null)
                {
                    Destroy(monster.gameObject.GetComponent<Rigidbody>());
                    monster.gameObject.GetComponent<NavMeshAgent>().enabled = true;
                    monster.gameObject.GetComponent<Animator>().enabled = true;
                    monster.enabled = true;
                    _monsters.Remove(monster);
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<MonsterMovementController>(out MonsterMovementController monster))
        {
            if (monster != null && monster.enabled && !_monsters.Contains(monster))
            {
                _monsters.Add(monster);
                monster.enabled = false;
                Rigidbody _rb = monster.gameObject.AddComponent<Rigidbody>();
                _rb.constraints = RigidbodyConstraints.FreezeAll;
                monster.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                monster.gameObject.GetComponent<Animator>().enabled = false;
                GameObject freeze = Instantiate(FreezeVFX, other.transform);

                Destroy(freeze,1f);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<MonsterMovementController>(out MonsterMovementController monster))
        {
            if (monster != null && monster.enabled && !_monsters.Contains(monster))
            {
                _monsters.Add(monster);
                monster.enabled = false;
                Rigidbody _rb = monster.gameObject.AddComponent<Rigidbody>();
                _rb.constraints = RigidbodyConstraints.FreezeAll;
                monster.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                monster.gameObject.GetComponent<Animator>().enabled = false;
                GameObject freeze = Instantiate(FreezeVFX, other.transform);
                Destroy(freeze, 1f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<MonsterMovementController>(out MonsterMovementController monster))
        {
            if (monster != null )
            {
                Destroy(monster.gameObject.GetComponent<Rigidbody>());
                monster.gameObject.GetComponent<NavMeshAgent>().enabled = true;
                monster.gameObject.GetComponent<Animator>().enabled = true;
                monster.enabled = true;
                if (_monsters.Contains(monster))
                {
                    _monsters.Remove(monster);
                }
            }
        }
    }
}

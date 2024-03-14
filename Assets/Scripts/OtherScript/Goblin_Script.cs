using RPGCharacterAnims.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Goblin_Script : MonoBehaviour
{

    public int tokenStealAmount = 1;
    public float healthReductionPercentage = 0.2f;
    [SerializeField] Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        PerformJump();
    }

    private void PerformJump()
    {
        _anim.SetTrigger("Jump");
        StartCoroutine(Jump());
    }

    IEnumerator Jump()
    {
        yield return new WaitForSeconds(1f);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("Goblin Triggered...");
            // Reduce player's token inventory and health
            //......MOHINI..........     PlayerInventory_Script playerInventory = other.gameObject.GetComponent<PlayerInventory_Script>();
            //......MOHINI..........     playerInventory.RemoveTokens(tokenStealAmount);
            _anim.SetTrigger("Snatch");
            HealthScript playerHealth = other.gameObject.GetComponent <HealthScript>();
            playerHealth.ReduceHealth(healthReductionPercentage);
            Destroy(gameObject);
        }
    }
}
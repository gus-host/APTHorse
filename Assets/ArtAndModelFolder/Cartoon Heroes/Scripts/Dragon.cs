using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dragon : MonoBehaviour
{
    public int HP = 100;
    public Slider healthBar;

    public Animator animator;

    private void Update()
    {
        healthBar.value = HP;
    }
    public void TakeDamage(int DamageAmount)
    {
        HP -= DamageAmount;
        print("HP :" + HP);
        if (HP <= 0)
        {
            animator.SetTrigger("die");
            //Play Death Animation
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            animator.SetTrigger("damage");
            print("Damage");
            //Play Get Hit Animation
        }
    }
}

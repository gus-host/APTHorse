using RPGCharacterAnims.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void Walk(bool walk)
    {
        anim.SetBool(AnimationTags.WALK_PARAMETER, walk);
    }

    public void BlockEnemy(bool block)
    {
        anim.SetBool(AnimationTags.BLOCK_PARAMETER, block);
    }

    public void Attack1()
    {
        anim.SetTrigger(AnimationTags.ATTACK_TRIGGER1);
    }

    public void Attack2()
    { 
       
        anim.SetTrigger(AnimationTags.ATTACK_TRIGGER2);
    }

    public void Attack3()
    {
        anim.SetTrigger(AnimationTags.ATTACK_TRIGGER3);
    }
    public void PerformJump()
    {
        anim.SetTrigger(AnimationTags.Jump_TRIGGER4);
    }

    public void Shield()
    {
        anim.SetTrigger(AnimationTags.Shield_TRIGGER4);
    }

    void FreezeAnimation()
    {
        anim.speed = 0;
    }
    public void WalkAnimation(Vector3 Movement)
    {
       anim.SetFloat("InputX", Movement.x);
       anim.SetFloat("InputY", Movement.z);
    }

    void UnFreezeAnimation()
    {
        anim.speed = 1f;
    }
}

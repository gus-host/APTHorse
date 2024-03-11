using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAnimation : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Walk(bool walk)
    {
        anim.SetBool("isWalk",true);
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

    void FreezeAnimation()
    {
        anim.speed = 0;
    }

    void UnFreezeAnimation()
    {
        anim.speed = 1f;
    }
}

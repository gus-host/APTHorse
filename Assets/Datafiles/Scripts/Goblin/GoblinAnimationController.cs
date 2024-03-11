using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAnimationController : MonoBehaviour
{
    private Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void Walk(bool walk)
    {
        anim.SetTrigger(GoblinAnimationTags.WALK_PARAMETER);
    }
    public void Run(bool walk)
    {
        Debug.LogWarning("Running animation");
        anim.SetTrigger(GoblinAnimationTags.RUN_PARAMETER);
    }
    public void Idle()
    {
        anim.SetTrigger(GoblinAnimationTags.IDLE_PARAMETER);
    }

    public void PerformJump()
    {
        anim.SetTrigger(GoblinAnimationTags.JUMP_PARAMETER);
    }

    public void Snatch()
    {
        anim.SetTrigger(GoblinAnimationTags.SNATCH_PARAMETER);
    }    
    
    public void Dead()
    {
        anim.SetBool(GoblinAnimationTags.DEAD_PARAMETER, true);
    }

    public void WalkAnimation(Vector3 Movement)
    {
        anim.SetFloat("InputX", Movement.x);
        anim.SetFloat("InputY", Movement.z);
    }
}

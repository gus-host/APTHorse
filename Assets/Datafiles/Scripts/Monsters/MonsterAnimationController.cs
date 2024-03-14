using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimationParameter(string parameterName, bool value)
    {
        animator.SetBool(parameterName, value);
    }

    public void SetAnimationParameter(string parameterName)
    {
        animator.SetTrigger(parameterName);
    }
}

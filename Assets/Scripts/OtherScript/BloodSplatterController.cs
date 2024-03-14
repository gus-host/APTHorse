using UnityEngine;

public class BloodSplatterController : MonoBehaviour
{
    public BloodSplatterController BSC;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayBloodSplatter()
    {
        animator.SetTrigger("PlayBloodSplatter");
    }
}

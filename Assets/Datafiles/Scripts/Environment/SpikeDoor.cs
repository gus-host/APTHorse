using UnityEngine;

public class SpikeDoor : MonoBehaviour
{

    public bool checkForOrb = false;
    public bool checkifFighted = false;

    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IntrantThirdPersonController>(out IntrantThirdPersonController playeController))
        {
            if (playeController._collectedOrbs==1 && playeController._fightedKaire && checkForOrb && checkifFighted)
            {
                animator.SetTrigger("Open");
            }else if(playeController._collectedOrbs >= 1)
            {
                animator.SetTrigger("Open");
            }
        }else if (other.gameObject.CompareTag(Tags.Kaire_TAG))
        {
            animator.SetTrigger("Open");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.PLAYER_TAG) && other.gameObject.TryGetComponent<IntrantThirdPersonController>(out IntrantThirdPersonController playeController) && checkForOrb)
        {
            if (!playeController._collectedLastOrb)
            {
                animator.SetTrigger("Close");
            }
        }
        else if (other.gameObject.CompareTag(Tags.Kaire_TAG))
        {
            animator.SetTrigger("Close");
        }
        else if (other.gameObject.CompareTag(Tags.PLAYER_TAG) && !checkForOrb)
        {
            animator.SetTrigger("Close");
        }
    }
}

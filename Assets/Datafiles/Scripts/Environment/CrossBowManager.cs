using UnityEngine;

public class CrossBowManager : MonoBehaviour
{
    public CrossBowController[] crossBowController;

    public bool isTrigger = false;
    public bool isCollision = false;

    private void OnCollisionEnter(Collision other)
    {
        if(isCollision)
        {
            if(other.gameObject.TryGetComponent<IntrantThirdPersonController>(out IntrantThirdPersonController player))
            {
                if(player != null)
                {
                    foreach (var crossbow in crossBowController)
                    {
                        crossbow.gameObject.SetActive(true);
                    }
                }
            }
        }
        
    }

    private void OnCollisionExit(Collision other)
    {
        if(isCollision)
        {
            if (other.gameObject.TryGetComponent<IntrantThirdPersonController>(out IntrantThirdPersonController player))
            {
                if (player != null)
                {
                    foreach (var crossbow in crossBowController)
                    {
                        crossbow.gameObject.SetActive(false);
                    }
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isTrigger)
        {
            if (other.gameObject.TryGetComponent<IntrantThirdPersonController>(out IntrantThirdPersonController player))
            {
                if (player != null)
                {
                    foreach (var crossbow in crossBowController)
                    {
                        crossbow.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isTrigger)
        {
            if (other.gameObject.TryGetComponent<IntrantThirdPersonController>(out IntrantThirdPersonController player))
            {
                if (player != null)
                {
                    foreach (var crossbow in crossBowController)
                    {
                        crossbow.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}

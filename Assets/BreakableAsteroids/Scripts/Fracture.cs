using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracture : MonoBehaviour
{
    [Tooltip("\"Fractured\" is the object that this will break into")]
    public GameObject fractured;
    
    public GameObject brokenFX;

    public GameObject glimmerArtifact;
    public void FractureObject()
    {
        GameObject blastFx = new GameObject();
        GameObject fractureRock = new GameObject();
        
        if(fractured != null)
        {
            fractureRock = Instantiate(fractured, transform.position, transform.rotation); //Spawn in the broken version
        }
        
        if(brokenFX != null )
        {
            blastFx = Instantiate(brokenFX, transform.position, transform.rotation); //Spawn in the broken version
        }

        Destroy(blastFx, 10f);
        Destroy(fractureRock, 50f);

        if(glimmerArtifact != null)
        {
            Instantiate(glimmerArtifact, transform.position, transform.rotation); //Spawn in the broken version
        }
        gameObject.SetActive(false);
        //Destroy(gameObject); //Destroy the object to stop it getting in the way
    }
}

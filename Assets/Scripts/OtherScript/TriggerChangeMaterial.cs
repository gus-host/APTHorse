using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChangeMaterial : MonoBehaviour
{
    public Material baseMaterial;
    public Material fadeMaterial;

    private void Start()
    {
        
    }
  // void OnTriggerEnter(Collider other)
  // {
  //     Debug.Log("Player has entered the trigger");
  //
  //     if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
  //     {
  //         GetComponent<Renderer>().material = fadeMaterial;
  //     }
  // }
  //
  // void OnTriggerExit(Collider other)
  // {
  //     Debug.Log("Player has exited the trigger");
  //
  //     if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
  //     {
  //         GetComponent<Renderer>().material = baseMaterial;
  //     }
  // }
}

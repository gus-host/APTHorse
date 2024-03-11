using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
            if(collision.gameObject.tag == "testtag")
        {
            Debug.Log("On collision");
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "testtag")
        {
            Debug.Log("On trigger");
        }
    }
}

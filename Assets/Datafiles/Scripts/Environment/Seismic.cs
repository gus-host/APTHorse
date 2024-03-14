using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seismic : MonoBehaviour
{
    public float magnitude = 50f; // Magnitude of the earthquake
    public float duration = 20.0f; // Duration of the earthquake in seconds

    private Vector3 originalPosition;
    private float timer = 0.1f;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {

            // Generate a random displacement within the specified magnitude
            Vector3 displacement = new Vector3(Random.Range(-magnitude, magnitude), 0, Random.Range(-magnitude, magnitude));

            // Apply the displacement to the object's position
            transform.position = originalPosition + displacement;

            timer += Time.deltaTime;
            // Reset the object's position when the earthquake ends
            transform.position = originalPosition;
        
    }
}

using UnityEngine;

public class LightDipper : MonoBehaviour
{
    public Light targetLight;
    public float dipDuration = 1.0f; // Duration of the dip effect in seconds
    public float minIntensity = 0.0f; // Minimum intensity when dipped
    public float maxIntensity = 1.0f; // Maximum intensity when not dipped
    public bool startDipped = false; // Start with the light dipped

    private float initialIntensity;
    private bool isDipped = false;
    private float timer = 0.0f;

    private void Start()
    {
        if (targetLight == null)
        {
            targetLight = GetComponent<Light>();
        }

        initialIntensity = targetLight.intensity;

        if (startDipped)
        {
            Dip();
        }
    }

    private void Update()
    {
        // Check for input or trigger to dip the light (e.g., space key)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isDipped)
            {
                Undip();
            }
            else
            {
                Dip();
            }
        }

        // Update the dip effect if active
        if (isDipped)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / dipDuration);
            targetLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);

            if (timer >= dipDuration)
            {
                timer = 0.0f;
                isDipped = false;
            }
        }
    }

    private void Dip()
    {
        isDipped = true;
        timer = 0.0f;
        targetLight.intensity = minIntensity;
    }

    private void Undip()
    {
        isDipped = true;
        timer = 0.0f;
        targetLight.intensity = maxIntensity;
    }
}
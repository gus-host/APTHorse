using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Highlight : MonoBehaviour
{
    public Image uiButton;
    public Color originalColor;
    public float scaleFactor = 1.2f;
    public float animationDuration = 0.5f;

    private Vector3 originalScale;

    public void PutOnAnimation()
    {
        StartCoroutine(Animate());
    }

    public void ChangeColor()
    {
        uiButton = GetComponent<Image>();
        originalColor = uiButton.color;
        uiButton.color = Color.grey;
    }

    private void OnEnable()
    {
        uiButton = GetComponent<Image>();
        originalColor = uiButton.color;
        uiButton.color = Color.grey;
    }

    IEnumerator Animate()
    {
        Debug.LogError("Animating...");
        uiButton = GetComponent<Image>();
        originalColor = uiButton.color;
        float elapsedTime = 0;
        float duration =  10f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            uiButton.DOColor(Color.red, animationDuration);

            yield return new WaitForSeconds(animationDuration);

            // Restore original color
            uiButton.DOColor(originalColor, animationDuration);

            yield return new WaitForSeconds(animationDuration);
        }
        yield return null;
    }

    private void OnDisable()
    {
        uiButton.color = originalColor;
    }
}

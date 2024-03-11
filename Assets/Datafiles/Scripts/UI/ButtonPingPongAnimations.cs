using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPingPongAnimations : MonoBehaviour
{
    public Button buttonToAnimate;
    public float animationDuration = 2.0f;
    public LeanTweenType easeType = LeanTweenType.linear;

    private Vector3 originalScale;

    void OnEnable()
    {
        buttonToAnimate = GetComponent<Button>();
        originalScale = buttonToAnimate.transform.localScale;

        // Start the animation when the scene starts
        PingPongAnimation();
    }

    void PingPongAnimation(float time = 2.0f)
    {
        LeanTween.scale(buttonToAnimate.gameObject, originalScale * 0.9f, animationDuration)
            .setEase(easeType)
            .setLoopPingPong();
    }
}

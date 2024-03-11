using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealingMap : MonoBehaviour
{
    public Material maskMaterial;
    public float revealSpeed = 0.1f;
    private float revealAmount = 0f;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
       // Debug.Log("Calling");
        Color maskColor = maskMaterial.color;
        maskColor.a = revealAmount;
        maskMaterial.color = maskColor;
        Graphics.Blit(source, destination, maskMaterial);
        revealAmount += revealSpeed * Time.deltaTime;
        revealAmount = Mathf.Clamp01(revealAmount);
    }



}

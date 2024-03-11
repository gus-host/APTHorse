using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SprintHandler : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    public IntrantThirdPersonController playerController;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        playerController.Sprint(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        playerController.Sprint(false);
    }
}

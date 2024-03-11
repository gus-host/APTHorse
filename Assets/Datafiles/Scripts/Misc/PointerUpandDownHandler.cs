using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerUpandDownHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    void IPointerDownHandler.OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        IntrantThirdPersonController.instance._jumpButtonPressed = true;
        Debug.LogError("Jump pressed");
    }

    void IPointerUpHandler.OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
    {
        //PlayerMovement_Manager1.instance._jumpButtonPressed = false;
        Debug.LogError("Jump Released");
    }
}

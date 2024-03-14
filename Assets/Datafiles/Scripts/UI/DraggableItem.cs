using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum WeaponType
{
    Hammer,
    Staff,
    Dagger
}

public class DraggableItem : MonoBehaviour, IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public WeaponType weaponType;
    public Transform parentAfterDrag;
    public Transform root;
    [SerializeField] Transform tempParent;
    RectTransform rectTransform;
    CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        parentAfterDrag = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.LogError("BeginDrag");
        canvasGroup.blocksRaycasts = false;
        GetComponent<Image>().raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.LogError("Drag");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.LogError("EndDrag");
        transform.SetParent(parentAfterDrag);
        canvasGroup.blocksRaycasts = true;
        GetComponent<Image>().raycastTarget = true;
    }

}

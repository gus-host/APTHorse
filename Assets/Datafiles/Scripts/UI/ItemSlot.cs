using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    RectTransform rectTransform;
    public bool llumanaMagic;
    public bool filledLlumanaMagic;
    public bool MalumanaMagic;
    public bool filledMalumanaMagic;
    public bool MainSlot;

    public Image _fill;

    public float _fillAmount;

    public CustomToast _customToast;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(!llumanaMagic && !MalumanaMagic)
        {
            if (eventData.pointerDrag != null)
            {
                GameObject dropped = eventData.pointerDrag;
                DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
                draggableItem.parentAfterDrag = transform;
            }
        }else if(llumanaMagic)
        {
            if (eventData.pointerDrag != null)
            {
                GameObject dropped = eventData.pointerDrag;
                DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
                if(!draggableItem.parentAfterDrag.GetComponent<ItemSlot>().MainSlot){
                    return;
                }
                if (draggableItem.weaponType == WeaponType.Hammer && _fillAmount <= 1f)
                {
                    Debug.LogError("Increasing");
                    _fillAmount += 0.1f;
                    _fill.fillAmount = _fillAmount;
                    if (_fillAmount >= 1)
                    {
                        filledLlumanaMagic = true;
                    }
                }
                else if(_fillAmount >= 1f)
                {
                    _customToast._text.text = "Filled llumana Magic";
                    _customToast.ShowToast();
                }
            }
        }
        else if(MalumanaMagic)
        {
            if (eventData.pointerDrag != null)
            {
                GameObject dropped = eventData.pointerDrag;
                DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
                if (!draggableItem.parentAfterDrag.GetComponent<ItemSlot>().MainSlot)
                {
                    return;
                }
                if (draggableItem.weaponType == WeaponType.Hammer && _fillAmount <= 1f)
                {
                    Debug.LogError("Increasing");
                    _fillAmount += 0.1f;
                    _fill.fillAmount = _fillAmount;
                    if (_fillAmount >= 1)
                    {
                        filledMalumanaMagic = true;
                    }
                }
                else if (_fillAmount >= 1f)
                {
                    _customToast._text.text = "Filled Malumana Magic";
                    _customToast.ShowToast();
                }
            }
        }
      
    }
}

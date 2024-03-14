using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemsManager : MonoBehaviour
{
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemCount;
    public Image ItemImage;

    public void SetItem(string itemName, string itemCount, Sprite itemSprite)
    {
        ItemName.text = itemName;
        ItemCount.text = itemCount;
        ItemImage.sprite = itemSprite;
    }
}

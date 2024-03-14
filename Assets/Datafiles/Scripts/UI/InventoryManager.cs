using UnityEngine;

[System.Serializable]
public struct ItemDetails
{ 
    public string itemMint;
    public string itemName;
    public Sprite itemSprite; 
}

public class InventoryManager : MonoBehaviour
{
    public ItemDetails[] itemDetails;
    public ItemsManager itemManager;
    public Transform potionsTrans;

    void Awake()
    {
        foreach (var item in itemDetails)
        {
          
        }
    }
}
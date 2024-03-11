using UnityEngine;
using UnityEngine.UI;

public class MapIndexing : MonoBehaviour
{
   public GameObject _panelSelect;
   public int _itemIndex;
   public bool _selected = false;
   private void Start()
   {
      _itemIndex = int.Parse(gameObject.name);
      GetComponent<Button>().onClick.AddListener(Selected);
   }

   public void Selected()
   {
      _selected = true;
      //_panelSelect.SetActive(false);
      FindObjectOfType<UIManagerMainMenu>().StartGame();
   }
}

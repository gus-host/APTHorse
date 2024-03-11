using UnityEngine;
using UnityEngine.UI;

public class PvPPlayerUI : MonoBehaviour
{
    
    public static PvPPlayerUI instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] public Button _firstAttack;
    [SerializeField] public Button _secondAttack;                                                           
    [SerializeField] public Button _jump;
    [SerializeField] public Button _sprint;
    [SerializeField] public Button _shield;
    [SerializeField] public Button _wallBreakbtn;
    [SerializeField] public Button _grimoire;
    [SerializeField] public Button _grimoirBackBtn;
    [SerializeField] public Button _breakPotionCollect;
    [SerializeField] public Button _collect;
    [SerializeField] public Button _switchOnAlkemmana;
    [SerializeField] public Button _realeaseThera;
    [SerializeField] public Button _PlaceCodes;
    [SerializeField] public Button _place;
    [SerializeField] public Button _chargeMainFrame;
    [SerializeField] public Button _initiateBinarySequence;
    [SerializeField] public Button _map;
    
    
}

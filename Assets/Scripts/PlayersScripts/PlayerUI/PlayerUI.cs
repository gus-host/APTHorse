using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI instance;
    public Button _swordAttack_btn;
    public Button _sworkdHorizontalAttack_btn;
    public Button _shield_btn;
    public Button _jump_btn;
    public Button _wallBreakbtn;
    public Button _place;
    public Button _collect;
    public Button _breakPotionCollect;
    public Button _switchOnAlkemmana;
    public Button _PlaceCodes;
    public Button _grimoire;
    public Button _grimoirBackBtn;
    public Button _charge;
    public Button _chargeMainFrame;
    public Button _revive;
    public Button _realeaseThera;

    private void Awake()
    {
        instance = this;
    }
}

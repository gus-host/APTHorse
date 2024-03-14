using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CheckPoint
{
    origoCorridor,
    alkemiachamber,
    thewaterwaysofalkem,
    thetheracodetrap,
    theriddlingstairs,
    theasimanamaze,
    theasimanahex,
    thetowerofeventualities,
    Thegatewaytotheterathian,
    TowerofEventualitiesTerathian,
    TheTerathianAbyss,
    ManaMussenden,
    ManaWalkways,
    ManaMacabreValley,
    TheTerathianEpimanaPowerPoint,
    TheDispellingLabrinyth,
    TheTerathianChasm,
}

[System.Serializable]
public struct TwoValues
{
    public string checkPointName;
    public int index;

    public TwoValues(string checkPointName, int index)
    {
        this.checkPointName = checkPointName;
        this.index = index;
    }
}
public class CheckpointTag : MonoBehaviour
{
    public CheckPoint checkPoint;
    
    [SerializeField]
    public TwoValues CheckPointInfo;

    private void Start()
    {
        gameObject.name = CheckPointInfo.checkPointName;
    }
}

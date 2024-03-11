using System.Collections;
using UnityEngine;

public class NavPlayer : MonoBehaviour
{
    public GameObject []_arrowAnim;
    public GameObject []_arrowAnimSetTwo;


    [Header("Week-1 Bools")] 
    public bool postRecolettaMirror = false;
    public bool noCondition = false;
    public bool postAlkemana = false;
    public bool postHotspot = false;
    public bool postMainframe = false;

    [Header("Week-2 Bools")]
    public bool _noOrb = false;
    public bool _threeOrb = false;

    [Header("Week-3 Bools")]
    public bool _example = false;
    private void OnTriggerEnter(Collider other)
    {
        #region Week-1
        if(GameManager.instance._mapName == MAPName.WEEK1)
        {
            if (postRecolettaMirror)
            {
                if (other.TryGetComponent<IntrantThirdPersonController>(out IntrantThirdPersonController _player))
                {
                    if(_player._beenToRecoletteMirror == true)
                    {
                        StartCoroutine(PlayAnim());
                    }
                }
            }

            if (noCondition)
            {
                StartCoroutine(PlayAnim());
            }
            
            if (postAlkemana)
            { 
                if (other.TryGetComponent<IntrantThirdPersonController>(out IntrantThirdPersonController _player))
                { 
                    if(_player._beenInAlkemiaChamber == true) 
                    { 
                        StartCoroutine(PlayAnim());
                    }
                }
            }

            if (postHotspot)
            {
                if (other.TryGetComponent<IntrantThirdPersonController>(out IntrantThirdPersonController _player))
                { 
                    if(_player._placedTheraCode == true) 
                    { 
                        StartCoroutine(PlayAnim());
                    }
                }
            }
            
            if (postMainframe)
            {
                if (other.TryGetComponent<IntrantThirdPersonController>(out IntrantThirdPersonController _player))
                { 
                    if(_player._mainFrameEnabled == true) 
                    { 
                        StartCoroutine(PlayAnim());
                    }
                }
            }
        }
        #endregion

        #region Week-2
        else if(GameManager.instance._mapName == MAPName.WEEK2)
        {
            if (_noOrb && other.gameObject.TryGetComponent<IntrantThirdPersonController>(out IntrantThirdPersonController _playerController))
            {
                if(_playerController._collectedOrbs == 1){
                    foreach (var obj in _arrowAnim)
                    {
                        obj.SetActive(true);
                    }
                }
                else
                {
                    foreach (var obj in _arrowAnimSetTwo)
                    {
                        obj.SetActive(true);
                    }
                }
            }
            
            if (_threeOrb)
            {
                if(other.gameObject.TryGetComponent<IntrantThirdPersonController>(out IntrantThirdPersonController _player))
                {
                    if(_player._collectedOrbs == 3){
                        foreach(var arrow in _arrowAnim)
                        {
                            arrow.SetActive(true);
                        }
                    }
                }
            }
        }
        #endregion

        #region Week-3
        else if (GameManager.instance._mapName == MAPName.WEEK3) {
            if (other.gameObject.CompareTag(Tags.PLAYER_TAG))
            {
                foreach(var arrow in _arrowAnim)
                {
                    arrow.SetActive(true);
                }
            }
        }
        #endregion
    }

    private IEnumerator PlayAnim()
    {
        foreach (GameObject g in _arrowAnim)
        {
            g.SetActive(true);
        }
        yield return new WaitForSeconds(10f);
        foreach (GameObject g in _arrowAnim)
        {
            g.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}

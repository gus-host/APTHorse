using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntrantPlayerTaskManager : MonoBehaviour
{
    public Button []_throwingStar;
    public Stack<GameObject> _throwingStarUsed;
    public TMP_Text _moves;
    public Button _exit;

    public bool foundThrowingStar = false;
    public bool allMovesUsed = false;

    public int _cuurentMovesCount = 5;
    public int _totalMovesCount = 5;

    private void Start()
    {
        _cuurentMovesCount = _totalMovesCount;
        _moves.text = _cuurentMovesCount.ToString() + "/" + _totalMovesCount.ToString();
        for (int i = 0 ; i < _throwingStar.Length; i++)
        {
            int calcProb = Random.Range(0, 2);
            if (calcProb == 0 && !foundThrowingStar)
            {
                _throwingStar[i].onClick.AddListener(() => {
                    LuckyStar();
                });
            }
            else
            {
                _throwingStar[i].onClick.AddListener(() => {
                    NormalStar();
                });
            }
        }
        _exit.onClick.AddListener(() =>
        {
            GetComponent<IntrantThirdPersonController>()._mirasChallengePanel.SetActive(false);
        });
    }

    private void Update()
    {
        if (_cuurentMovesCount < 1 && !allMovesUsed || foundThrowingStar)
        {
            foreach (var task in _throwingStar)
            {
                task.interactable = false;
                _exit.interactable = true;
                allMovesUsed = true;
            }
        }
    }

    private void LuckyStar()
    {
        Debug.LogError("LuckyStar");
        foundThrowingStar = true;
        _moves.text = "Found correct star";
        GetComponent<IntrantThirdPersonController>()._foundThrowingStar = true;
    }
    private void NormalStar()
    {
        Debug.LogError("NormalStar");
        _cuurentMovesCount--;
        _moves.text = _cuurentMovesCount.ToString() + "/" + _totalMovesCount.ToString();
    }
}

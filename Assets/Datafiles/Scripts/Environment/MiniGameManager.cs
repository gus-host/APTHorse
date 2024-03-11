using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager _instance;
    public GameObject[] spawnPoints;
    public GameObject _tpEffect;
    public GameObject _minimapPointerOfOrb;
    public GameObject _orbsOfKinesis;

    private void Start()
    {
        _instance = this;
        if(_minimapPointerOfOrb != null && _orbsOfKinesis != null)
        {
            GameObject _minimapPointer = Instantiate(_minimapPointerOfOrb);
            _minimapPointer.GetComponent<PlayerMinimapPointer>().target = _orbsOfKinesis.transform;
        }
    }

    public void InitTPEffect()
    {
        StartCoroutine(TP());
    }

    IEnumerator TP()
    {
        _tpEffect.SetActive(true);
        yield return new WaitForSeconds(2f);
        _tpEffect.SetActive(false);
    }
}

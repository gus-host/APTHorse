using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Button _enableMapbtn;
    [SerializeField] private Button _disableMapBtn;
    [SerializeField] private List<Sprite> _maps = new List<Sprite>();
    [SerializeField] private Image _map;
    private void Awake()
    {
        SetListeners();
    }

    private void SetListeners()
    {
        _enableMapbtn.onClick.AddListener(EnableMapView);
        _disableMapBtn.onClick.AddListener(DisableMapView);
    }

    private void EnableMapView()
    {
        _disableMapBtn.gameObject.SetActive(true); 
        _map.gameObject.SetActive(true);
        _map.sprite = _maps[0];
        _enableMapbtn.gameObject.SetActive(false);
        Time.timeScale = 0f;
        Debug.LogError("pausing game..");
    }

    private void DisableMapView()
    {
        _enableMapbtn.gameObject.SetActive(true);
        _disableMapBtn.gameObject.SetActive(false); 
        _map.gameObject.SetActive(false);
        _map.sprite = _maps[0];
        Debug.LogError("Resuming game..");
        Time.timeScale = 1f;
    }
}

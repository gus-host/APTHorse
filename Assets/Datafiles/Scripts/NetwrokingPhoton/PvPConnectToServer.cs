using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PvPConnectToServer : MonoBehaviourPunCallbacks
{
    public TMP_InputField _playerName;
    public Button _continue;
    public TMP_Text _continuetext;
    public GameObject _profilePanel;
    public GameObject _lobbyManager;
    
    private void Start()
    {
        _continue.onClick.AddListener(Connect);
        PhotonNetwork.Disconnect();
    }

    private void Connect()
    {
        if (!string.IsNullOrEmpty(_playerName.text))
        {
            Debug.LogError($"Name {_playerName.text}");
            PhotonNetwork.NickName = _playerName.text;
            _continuetext.text = "connecting..";
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.LogError($"Empty {_playerName.text}");
        }
    }

    public override void OnConnectedToMaster()
    {
        _lobbyManager.SetActive(true);
        _continuetext.text = "starting game";
    }
}

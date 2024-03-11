using Invector.vCharacterController;
using Photon.Pun;
using UnityEngine;

public class PvPPlayerManager : MonoBehaviourPunCallbacks
{

    public GameObject _playerCanvas;

    

    public void Start()
    {
        //SetLocalPlayer

        _playerCanvas.SetActive(photonView.IsMine);
        
        Debug.Log("OnClient Called");
        GetComponent<vThirdPersonController>().localPlayer = photonView.IsMine;
        GetComponent<vThirdPersonInput>().localPlayer = photonView.IsMine;
        Debug.Log($"OnClient Called with ownership {photonView.IsMine}");
    }
}
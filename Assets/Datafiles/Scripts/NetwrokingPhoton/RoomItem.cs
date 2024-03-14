using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public TMP_Text _roomName;
    public TMP_Text _roomMember;
    private PvPPhotonLobbyManager _lobbyManager;

    private void Start()
    {
        _lobbyManager = FindObjectOfType<PvPPhotonLobbyManager>();
        GetComponent<Button>().onClick.AddListener(JoinRoom);
    }

    public void SetRoomName(string name)
    {
        _roomName.text = name;
    }
    public void SetRoomMember(string name)
    {
        _roomMember.text = name;
    }
    private void JoinRoom()
    {
        _lobbyManager.JoinRoom(_roomName.text);
    }
    
    
}

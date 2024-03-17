using Photon.Pun;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerInstance : MonoBehaviourPunCallbacks
{
    
    public static ServerInstance Instance;

    private void Start()
    {
        Instance = this;
    }
    public IEnumerator InitSceneSwitchRPC()
    {
        if(!PhotonNetwork.InRoom)
        {
            Debug.LogError("Not in room");
            yield return null;
        }
        yield return new WaitForSeconds(5f);
        Debug.LogError("InitSceneSwitch");
        photonView.RPC("SwitchToRace", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    private void SwitchToRace()
    {
        //We will store the variables
        Debug.LogError("Loading scene");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("HorseJockey");
        }
    }

    public void RPCToggleSwitch(string data)
    {
        if (!PhotonNetwork.InRoom)
        {
            Debug.LogError("Not in room");
            return ;
        }
        Debug.LogError("RPCToggleSwitch");
        photonView.RPC("ToggleSwitch", RpcTarget.AllBufferedViaServer, data);
    }

    [PunRPC]
    public void ToggleSwitch(string data)
    {
        JSONNode node = JSONNode.Parse(data);
        Debug.LogError($"JSON {data}");
        int raceId = node[1];
        JSONNode randomAcceleration = node[2];
        JSONNode randomHurdles = node[3];

        List<RacePlayer> players = new();
        for (int i = 0; i < randomAcceleration.Count; i++)
        {
            List<float> playerHurdles = new();
            for (int j = 0; j < randomHurdles[i].Count; j++)
            {
                playerHurdles.Add(int.Parse(randomHurdles[i][j].Value) / 100);
            }

            RacePlayer player = new()
            {
                acceleration = (float)int.Parse(randomAcceleration[i].Value) / 100,
                hurdles = playerHurdles
            };
            players.Add(player);
        }
        for (int i = 0; i< players.Count; i++)
        {
            Debug.LogError($"acceleration {players[i].acceleration}");
            Debug.LogError($"randomAcceleration {randomAcceleration[i].AsInt}");
            Debug.LogError($"randomAcceleration Parse {int.Parse(randomAcceleration[i].Value)}");
            Debug.LogError($"hurdles {players[i].hurdles}");
        }
    }


/*    private void RPCGenerateSpawnPoints()
    {
        photonView.RPC("GenerateSpawnPoints", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    public void GenerateSpawnPoints()
    {
        Debug.LogError("GenerateSpawnPoints");
        if (racePlayer.Count > 1)
        {
            Debug.LogError($"Player count {racePlayer.Count}");
        }
        else if (racePlayer.Count < 1)
        {
            Debug.LogError($"Player count 0");
        }
        for (int index = 0; index < racePlayer.Count; index++)
        {
            Debug.LogError("GenerateSpawnPoints");
            if (racePlayer[index].joinedRaceInfo.playerName == PhotonNetwork.NickName)
            {
                spawnAt = index; //i-th spot
            }
        }
    }*/
}

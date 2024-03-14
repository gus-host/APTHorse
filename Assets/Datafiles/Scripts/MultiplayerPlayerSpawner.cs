using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class MultiplayerPlayerSpawner : MonoBehaviourPunCallbacks
{
    public GameObject[] _playerPrefab;

    public GameObject[] _spawnPoints;

    public GameObject _spawnEffect;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        // Check if the local player is the master client
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.LocalPlayer.IsLocal)
        {
            Debug.LogError("Spawning Player");
            photonView.RPC("SpawnPlayer", RpcTarget.AllBuffered);
        }
        else
        {
            // For non-master clients or non-local players, set the spawn point index received from the master client.
            int spawnPointIndex = UnityEngine.Random.Range(0, _spawnPoints.Length);
            photonView.RPC("SetSpawnPointIndex", RpcTarget.AllBuffered, spawnPointIndex);
        }
    }

    [PunRPC]
    private void SetSpawnPointIndex(int index)
    {
        // Set the spawn point index received from the master client.
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //Debug.LogError("Setting Spawn Point Index");
            int spawnPointIndex = Mathf.Clamp(index, 0, _spawnPoints.Length - 1);
            photonView.Owner.CustomProperties["SpawnPointIndex"] = spawnPointIndex;
        }
    }

    [PunRPC]
    private void SpawnPlayer()
    {
        int spawnPointIndex = 0;

        Debug.LogError("Spawning Player");
        // Retrieve the spawn point index from room properties.
        if (PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient)
        {
            spawnPointIndex = 0;
        }
        else
        {
            object spawnpointInstance = PhotonNetwork.CurrentRoom.CustomProperties["SpawnpointIndex"];
            spawnPointIndex = (int)spawnpointInstance;
        }

        // Ensure the spawnPointIndex is within valid range
        if (spawnPointIndex < 0 || spawnPointIndex >= _spawnPoints.Length)
        {
            Debug.LogError("Invalid spawnPointIndex: " + spawnPointIndex);
            return;
        }

        // Spawn the player across the network.
        var playerPrefabIndex = UnityEngine.Random.Range(0, _playerPrefab.Length);
        var player = PhotonNetwork.Instantiate(_playerPrefab[spawnPointIndex].name, _spawnPoints[spawnPointIndex].transform.position, Quaternion.identity);

        HorseController _horseController = player.GetComponent<HorseController>();
        
        Hashtable Spawnpoint = new Hashtable();
        Spawnpoint.Add("SpawnpointIndex", ++spawnPointIndex);
        PhotonNetwork.CurrentRoom.SetCustomProperties(Spawnpoint);

        // Access a custom room property
    }
}

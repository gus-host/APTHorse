using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class MultiplayerPlayerSpawner : MonoBehaviourPunCallbacks
{
    public GameObject[] _playerPrefab;

    public GameObject[] _spawnPoints;

    public GameObject _spawnEffect;

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Hashtable Spawnpoint = new Hashtable();
            Spawnpoint.Add("ValuesSet", false);
            Spawnpoint.Add("SpawnPointIndex1", false);
            Spawnpoint.Add("SpawnPointIndex2", false);
            Spawnpoint.Add("SpawnPointIndex3", false);
            Spawnpoint.Add("SpawnPointIndex4", false);
            PhotonNetwork.CurrentRoom.SetCustomProperties(Spawnpoint);
        }
    }

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        // Check if the local player is the master client

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("Spawning Player");
            photonView.RPC("SpawnPlayer", RpcTarget.AllBufferedViaServer);
        }
        else
        {
            // For non-master clients or non-local players, set the spawn point index received from the master client.
            int spawnPointIndex = UnityEngine.Random.Range(0, _spawnPoints.Length);
            photonView.RPC("SetSpawnPointIndex", RpcTarget.AllBufferedViaServer, spawnPointIndex);
        }
    }

    [PunRPC]
    private void SetSpawnPointIndex(int index)
    {
        // Set the spawn point index received from the master client.
        if (!PhotonNetwork.IsMasterClient)
        {
            //Debug.LogError("Setting Spawn Point Index");
            int spawnPointIndex = Mathf.Clamp(index, 0, _spawnPoints.Length - 1);
            photonView.Owner.CustomProperties["SpawnPointIndex"] = spawnPointIndex;
        }
    }

    [PunRPC]
    private void SpawnPlayer()
    {
        // Spawn the player across the network.
        var player = PhotonNetwork.Instantiate(_playerPrefab[WalletManager.Instance.EquippedHorseId].name, _spawnPoints[WalletManager.Instance.spawnAt].transform.position, Quaternion.identity);

        HorseController _horseController = player.GetComponent<HorseController>();

        // Access a custom room property
    }
}

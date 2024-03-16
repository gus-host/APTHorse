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
        int spawnPointIndex = 0;

        Debug.LogError("Spawning Player");
        // Retrieve the spawn point index from room properties.
        if (PhotonNetwork.IsMasterClient)
        {
            spawnPointIndex = 0;
        }
        else
        {
            for (int i = 1; i < 5; i++) // Start from index 2 as per your custom properties
            {
                bool spawnpointInstance = (bool)PhotonNetwork.CurrentRoom.CustomProperties["SpawnPointIndex" + i];
                if (!spawnpointInstance)
                {
                    // If the spawn point is available, assign its index and mark it as used
                    spawnPointIndex = i;
                    PhotonNetwork.CurrentRoom.CustomProperties["SpawnPointIndex" + i] = true;
                    break;
                }
            }
            /*            //Get the bool value
                        bool spawnpointInstance = (bool)PhotonNetwork.CurrentRoom.CustomProperties["SpawnPointIndexTwo"];
                        //Check for false
                        if (!spawnpointInstance)
                        {
                            //If not used assign its value to index make it used
                            spawnPointIndex = 1;
                            PhotonNetwork.CurrentRoom.CustomProperties["SpawnPointIndexTwo"] = true;
                        }
                        else if(spawnpointInstance)
                        {
                            //Get the bool value
                            spawnpointInstance = (bool)PhotonNetwork.CurrentRoom.CustomProperties["SpawnPointIndexThree"];
                            //Check for false
                            if (!spawnpointInstance)
                            {
                                //If not used assign its value to index make it used
                                spawnPointIndex = 2;
                                PhotonNetwork.CurrentRoom.CustomProperties["SpawnPointIndexThree"] = true;
                            }
                            else if (spawnpointInstance)
                            {
                                //Get the bool value
                                spawnpointInstance = (bool)PhotonNetwork.CurrentRoom.CustomProperties["SpawnPointIndexfour"];
                                //Check for false
                                if (!spawnpointInstance)
                                {
                                    //If not used assign its value to index make it used
                                    spawnPointIndex = 3;
                                    PhotonNetwork.CurrentRoom.CustomProperties["SpawnPointIndexfour"] = true;
                                }
                                else if (spawnpointInstance)
                                {
                                    //Get the bool value
                                    spawnpointInstance = (bool)PhotonNetwork.CurrentRoom.CustomProperties["SpawnPointIndexfive"];
                                    //Check for false
                                    if (!spawnpointInstance)
                                    {
                                        //If not used assign its value to index make it used
                                        spawnPointIndex = 4;
                                        PhotonNetwork.CurrentRoom.CustomProperties["SpawnPointIndexfive"] = true;
                                    }
                                }
                            }
                        }*/
        }

        // Ensure the spawnPointIndex is within valid range
        if (spawnPointIndex < 0 || spawnPointIndex >= _spawnPoints.Length)
        {
            Debug.LogError("Invalid spawnPointIndex: " + spawnPointIndex);
            return;
        }

        // Spawn the player across the network.
        var player = PhotonNetwork.Instantiate(_playerPrefab[spawnPointIndex].name, _spawnPoints[spawnPointIndex].transform.position, Quaternion.identity);

        HorseController _horseController = player.GetComponent<HorseController>();

        // Access a custom room property
    }
}

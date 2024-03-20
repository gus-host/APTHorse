using Photon.Pun;
using UnityEngine;

public enum HorseSer
{
    H1,
    H2,
    H3,
    H4,
    H5
}


public class WayPointList : MonoBehaviour
{
    public HorseSer horseSer;

    public Transform []_wayPoint;

    public Transform []_puddleSpts;
    public Transform []_puddleSpts2;
    public Transform []_puddleSpts3;

    public GameObject obstaclePrefab;

    int i = 1;
    private void Start()
    {
        foreach (Transform go in _wayPoint)
        {
            go.name = "Waypoint " + i++;
        }
    }

    //horse will call this before starting every lap
    public void SpawnObstacleAtPercentage(float spawnPercentage, int index)
    {
        int puddleSpts = (int)Mathf.Ceil(spawnPercentage);
        if(index ==  0)
        {
            PhotonNetwork.Instantiate(obstaclePrefab.name, _puddleSpts[puddleSpts].position, Quaternion.identity);
        }else if(index == 1)
        {
            PhotonNetwork.Instantiate(obstaclePrefab.name, _puddleSpts2[puddleSpts].position, Quaternion.identity);
        }else if (index == 2)
        {
            PhotonNetwork.Instantiate(obstaclePrefab.name, _puddleSpts3[puddleSpts].position, Quaternion.identity);
        }
    }
}

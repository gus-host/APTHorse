using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    SpawnManager spawnManager;
    public Transform[] healthKitSpawnPoint;
    public List<int> spawnKitRandomLocation ;
    void Start()
    {
        spawnHealthKit();
    }
    void spawnHealthKit()
    {
        int increament=0;
        for(int i = 0; i < 5+increament; i++)
         {
            Debug.Log("randomlocation");
            int random = Random.Range(0, healthKitSpawnPoint.Length);
            for (int j = 0; j <5; i++)
            {
                Debug.Log("randomlocation1");
                if (spawnKitRandomLocation[j] == random)
                {
                    Debug.Log("randomlocation2");
                    increament++;
                    break;
                }
                else 
                {
                    Debug.Log("randomlocation3");
                    spawnKitRandomLocation.Add(random);
                }
            
             }
         }

    }
   
}

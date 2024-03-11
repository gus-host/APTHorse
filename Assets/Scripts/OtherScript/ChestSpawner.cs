using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChestCategory
{
    Normal,
    Adacode,
    Blast,
    MonsterEmergence
}


public class ChestSpawner : MonoBehaviour
{
    public ChestType chestType;
    public GameObject Chest;
    public float spawnRadius = 1f;
    public List<Vector3> _spawnPos;
    public List<GameObject> puddle;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            _spawnPos.Add(child.position);
        }

        SpawnChest();
    }

    private void SpawnChest()
    {
        Debug.Log("Player detected chest spawn init");
        int randomIndex = Random.Range(0, _spawnPos.Count);
        Debug.Log("RandomIndex " + randomIndex);
        Debug.LogFormat("Chest spawn index {0} and position {1} ", randomIndex, _spawnPos[randomIndex]);
        Vector3 spawnPosition = new Vector3(_spawnPos[randomIndex].x, _spawnPos[randomIndex].y, _spawnPos[randomIndex].z);
        GameObject _Chest =Instantiate(Chest, spawnPosition, Quaternion.identity);
        _Chest.GetComponent<Chest_Script>()._ChestType = chestType;
        _Chest.GetComponent<Chest_Script>().puddle = puddle;
       gameObject.SetActive(false);
    }
}
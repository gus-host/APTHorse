using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum EnemySpawnerType{
    SpawnPoint,
    GroundEmergence
}


public class EnemySpawner : MonoBehaviour
{
    public EnemySpawnerType enemySpawnerType;   

    public GameObject[] enemyPrefab; 
    public GameObject[] spawnPosition;
    public Stack<GameObject> _SptsStack = new Stack<GameObject>();

    public GameObject[] spawnTrigger;
    
    public List<GameObject> _IngameEnemies = new List<GameObject>();

    public int maxSpawnCount = 1;
    public float spawnRate = 0;
    public float spawnTimer = 0;
    public int enemiesRange = 0;

    [Header("bool")]
    public bool _playerCrossed = false;
    public bool _usePooling = false;
    public bool _dipperSpawn = false;
    public bool _reachedLimit = false;

    [Header("Tunnel")] 
    public GameObject _door;
    public GameObject []_arrow;
    public Animator _tunnelDoorAnimation;

    [Header("Magic")]
    public GameObject _magic;


    [Header("Orbs")] 
    public GameObject OrbOfKinesis;

    [Header("Orbs")] 
    public GameObject _tornado;

    public FortressOfEventualitiesTriggerManagerEarthandWater fortressOfEventualities;

    private void OnEnable()
    {
        if(enemySpawnerType == EnemySpawnerType.GroundEmergence)
        {
            foreach(GameObject arrow in _arrow)
            {
                arrow.SetActive(true);
            }
        }
    }

    private void Start()
    {
        //default enemy range if 0
        if(enemiesRange == 0)
        {
            enemiesRange = 10;
        }

        if (enemySpawnerType == EnemySpawnerType.SpawnPoint)
        {
            SpawnEnemies();

            if (_usePooling && _dipperSpawn && !_reachedLimit)
            {
                Debug.LogError("DipperSpawn");
                int i = 0;
                if (i == maxSpawnCount)
                {
                    _reachedLimit = true;
                }
                while (i < maxSpawnCount)
                {
                    GameObject _enemy = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], spawnPosition[Random.Range(0, spawnPosition.Length)].transform.position, Quaternion.Euler(0, -90, 0));
                    _enemy.GetComponent<MonsterMovementController>().findEnemies = true;
                    _enemy.GetComponent<MonsterMovementController>()._radiusRange = enemiesRange;
                    _enemy.SetActive(false);
                    _IngameEnemies.Add(_enemy);
                    i++;
                }
                InvokeRepeating("SpawnRep",0,5f);
            }
        }else if(enemySpawnerType == EnemySpawnerType.GroundEmergence)
        {
            MonsterEmergence();
        }
    
    }

    private void SpawnRep()
    {
        if(_IngameEnemies.Count > 0)
        {
            _IngameEnemies[0].SetActive(true);
            _IngameEnemies.Remove(_IngameEnemies[0]);
        }
    }

    private IEnumerator Emerge()
    {
        _IngameEnemies[0].GetComponent<MonsterMovementController>().enabled = false;
        _IngameEnemies[0].GetComponent<NavMeshAgent>().enabled = false;
        yield return new WaitForSeconds(0.01f);
       
        yield return new WaitForSeconds(0.01f);
        _IngameEnemies[0].GetComponent<MonsterMovementController>().enabled = true;
        _IngameEnemies[0].GetComponent<NavMeshAgent>().enabled = true;
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_playerCrossed && !this.gameObject.CompareTag("PlayerStage"))
        {
            _playerCrossed = true;   
        }
    }
    private void Update()
    {

        try
        {
            foreach (var enemies in _IngameEnemies)
            {
                if (enemies == null)
                {
                    _IngameEnemies.Remove(enemies);
                }
            }
        }
        catch (Exception e)
        {

        }
        if (GameManager.instance._gameEnded)
        {
            this.gameObject.SetActive(false);
        }

        if (_IngameEnemies.Count < 1 && _door!=null)
        {
            _tunnelDoorAnimation.SetBool("Open", true);
            Destroy(_door,5f);
        }

        if (_IngameEnemies.Count < 1 && OrbOfKinesis!=null)
        {
            OrbOfKinesis.SetActive(true);
            GetComponent<Animator>().SetBool("Raise", true);
        }

        if (_IngameEnemies.Count < 1 && GetComponent<EmitterOrbsManager>() != null)
        {
            GetComponent<EmitterOrbsManager>().ReInitiate();
            Destroy(GetComponent<EnemySpawner>());
        }

        if(_IngameEnemies.Count<1 && fortressOfEventualities != null)
        {
            fortressOfEventualities.FightFinished();
            IntrantThirdPersonController.instance._fightedKaire = true;
            GetComponent<EnemySpawner>().enabled = false;
        }

        if(_IngameEnemies.Count < 1 && enemySpawnerType == EnemySpawnerType.GroundEmergence)
        {
            Destroy(gameObject);
        }

        if(_tornado!=null)
        {
            if(_IngameEnemies.Count < 1){
                _tornado.SetActive(false);
                IntrantThirdPersonController.instance._killedBossMonster = true;
            }
        }

        if (_IngameEnemies.Count < 1 && _magic!=null)
        {
            Vector3 pos = new Vector3(_magic.transform.position.x, _magic.transform.position.y-15, _magic.transform.position.z);
            LeanTween.move(_magic, pos, 3f).setOnComplete(Disablemagic);
        }
    }

    private void Disablemagic()
    {
        _magic.SetActive(false);
    }

    public void MonsterEmergence()
    {
        while (_IngameEnemies.Count <= maxSpawnCount)
        {
            GameObject _enemy = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], spawnPosition[Random.Range(0, spawnPosition.Length)].transform.position, Quaternion.identity);
            _enemy.SetActive(false);
            _enemy.transform.position = new Vector3(_enemy.transform.position.x, _enemy.transform.position.y - 1, _enemy.transform.position.z);
            _enemy.GetComponent<MonsterMovementController>()._radiusRange = enemiesRange;
            _IngameEnemies.Add(_enemy);
        }
        InvokeRepeating("SpawnRepEmerge", 0, 8f);
    }

    private void SpawnRepEmerge()
    {
        float desiredY = _IngameEnemies[0].transform.position.y - 1.0f;
        LeanTween.moveY(_IngameEnemies[0], desiredY, 0.01f);
        _IngameEnemies[0].SetActive(true);
        desiredY = _IngameEnemies[0].transform.position.y + 1.0f;
        LeanTween.moveY(_IngameEnemies[0], desiredY, 1.0f);
        _IngameEnemies.Remove(_IngameEnemies[0]);
    }

    public void SpawnEnemies()
    {
        if (_usePooling && _dipperSpawn)
        {
            return;
        }

        _IngameEnemies.Clear();
        while (_IngameEnemies.Count < maxSpawnCount)
        {
            int index = Random.Range(0, spawnPosition.Length);
            if (_SptsStack.Count < 1)
            {
                GameObject _enemy = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], spawnPosition[index].transform.position, Quaternion.identity);
                _enemy.TryGetComponent<MonsterMovementController>(out MonsterMovementController _monster);
                if (_monster != null)
                {
                    _monster._radiusRange = enemiesRange;
                }
                _IngameEnemies.Add(_enemy);
                _SptsStack.Push(spawnPosition[index]);
            }
            if (!_SptsStack.Contains(spawnPosition[index]))
            {
                GameObject _enemy = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], spawnPosition[index].transform.position, Quaternion.identity);
                _enemy.TryGetComponent<MonsterMovementController>(out MonsterMovementController _monster);
                if (_monster != null)
                {
                    _monster._radiusRange = enemiesRange;
                }
                _IngameEnemies.Add(_enemy);
                _SptsStack.Push(spawnPosition[index]);
            }
        }
    }

    public void DestroyEnemies()
    {
        foreach (var enemy in _IngameEnemies)
        {
            Destroy(enemy);
        }
    }
}

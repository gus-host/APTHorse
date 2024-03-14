using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChestType
{
    Normal,
    AdacodeCryptex,
    Blast,
    MonsterEmergence
}

public class Chest_Script : MonoBehaviour
{
    public ChestType _ChestType;
    public static Chest_Script instance;
    
    public Animator Chest_Anim;
    
    public GameObject tokenPrefab;
    public GameObject goblinPrefab;
    public GameObject lightFx;
    public GameObject souvenirCopy;
    public GameObject goblinMinimapPointer;
    public GameObject chestPointerMinimap;
    public GameObject _adacode;
    public GameObject _blastVFX;
    public GameObject _pupleVFX;
    GameObject _puddlrainVfxRef;
    public List<GameObject> puddle;
    
    [Header("Variables")]
    public float goblinProbability = 0.5f;
    public float spawnRadius = 3f;
    public int ChestSpawn_Count;
    public int _maxGoblinCount = 5;
    public int _minGoblinCount = 2;
    public int _goblinCountInGame = 0;

    [Header("Bools")]
    public bool _chestExpired = false;
    public bool _spawnedAllGoblin = false;
    public bool _goblinSpawn = true;
    public bool _coinSpawn = true;
    public bool souvenirInstantiated = false;

    [Header("Testing ")]
    public bool _goblinSpawntesing = false;
    public bool _coinSpawntesing = false;

    int CalculateProbability;

    private void Start()
    {
        GameObject minimapPointer = Instantiate(chestPointerMinimap);
        minimapPointer.GetComponent<PlayerMinimapPointer>().target = transform;
        CalculateProbability = UnityEngine.Random.Range(0, 2);
        Debug.LogWarning("Probability "+ CalculateProbability);
        Chest_Anim = gameObject.GetComponent<Animator>();
        instance = this;
    }

    public void Open()
    {
        _chestExpired = true;
        Debug.Log("OPEN...");
        Chest_Anim.SetTrigger("isChestOpen");
        if (_ChestType == ChestType.Normal)
        {
            
            if(!_goblinSpawntesing && ! _coinSpawntesing)
            {
                if (CalculateProbability == 0 && _goblinSpawn)
                    StartCoroutine(InitiateGoblinEmergence());
                else if (CalculateProbability == 1 && _coinSpawn)
                    StartCoroutine(InitiateCoinEmergence());
            }
            else
            {
                if(_goblinSpawntesing)
                {
                    StartCoroutine(InitiateGoblinEmergence());
                }
                else if (_coinSpawntesing)
                {
                    StartCoroutine(InitiateCoinEmergence());
                }
            }
        }  else if(_ChestType == ChestType.AdacodeCryptex)
        {
            StartCoroutine(InitiateAdacodeEmergence());
        }else if(_ChestType == ChestType.Blast)
        {
            StartCoroutine(InitiateBlastEmergence());
        }else if (_ChestType == ChestType.MonsterEmergence)
        {
            StartCoroutine(InitiateMonsterEmergence());
        }
        
    }

    private IEnumerator InitiateMonsterEmergence()
    {
        yield return new WaitForSeconds(1f);
        _puddlrainVfxRef = Instantiate(_pupleVFX, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        foreach (var obj in puddle)
        {
            obj.SetActive(true);
        }
    }

    private IEnumerator InitiateBlastEmergence()
    {
        yield return new WaitForSeconds(1.5f);
        GameObject _obj = Instantiate(_blastVFX, transform.position, Quaternion.identity);
        yield return null;
    }

    //Instantiate Coin
    IEnumerator InitiateCoinEmergence()
    {
        yield return new WaitForSeconds(1.5f);
        
        GameObject _coin = Instantiate(tokenPrefab , new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        souvenirInstantiated = true;
        souvenirCopy = _coin;
        LeanTween.move(_coin, new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), 0.5f);
        LeanTween.scale(_coin,new Vector3(transform.localScale.x + 0.3f, transform.localScale.y + 0.3f, transform.localScale.z + 0.3f), 1f);
        Debug.LogWarning("Instantiated Coin" + _coin);
        yield return null;
        DestroyChest();
    }
    //Instantiate Adacode
    IEnumerator InitiateAdacodeEmergence()
    {
        yield return new WaitForSeconds(1.5f);

        GameObject _adacodeRef = Instantiate(_adacode, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        BoxCollider _adacodeBoxCollider = _adacodeRef.GetComponent<BoxCollider>();
        _adacodeBoxCollider.enabled = false;
        souvenirInstantiated = true;
        souvenirCopy = _adacodeRef;
        LeanTween.move(_adacodeRef, new Vector3(transform.position.x, transform.position.y + 4f, transform.position.z), 0.5f);
        LeanTween.scale(_adacodeRef, new Vector3(transform.localScale.x + 0.3f, transform.localScale.y + 0.3f, transform.localScale.z + 0.3f), 1f).setOnComplete(() => {
            _adacodeBoxCollider.enabled = true;
        });
        Debug.LogWarning("Instantiated Coin" + _adacodeRef);
        yield return new WaitForSeconds(1f);
        _adacodeRef.GetComponent<RotateandLevitate>().enabled = true;
        lightFx.SetActive(false);
        yield return null;
        DestroyChest();
    }
    
    
    IEnumerator InitiateGoblinEmergence()
    {
        souvenirInstantiated = true;
        int _tempGoblinCount = UnityEngine.Random.Range(_minGoblinCount, _maxGoblinCount);
        while (!_spawnedAllGoblin)
        {
            if (_goblinCountInGame <= _tempGoblinCount)
            {
                yield return new WaitForSeconds(1.5f);
                GameObject goblin = Instantiate(goblinPrefab, transform.position, Quaternion.identity);
                _goblinCountInGame++;
                GameObject _g = Instantiate(goblinMinimapPointer);
                _g.GetComponent<PlayerMinimapPointer>().target = goblin.transform;
                goblin.GetComponent<GoblinManager>().minimapPointer = _g; 
                Debug.LogWarning("Goblin " + _goblinCountInGame);
                //yield return new WaitForSeconds(5f);
            }
            else
            {
                //GetComponent<Collider>().enabled = false;
                _spawnedAllGoblin = true;
                yield return null;
            }
        }
        lightFx.SetActive(false);
        DestroyChest();
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.LogError($"Chest collided {other.gameObject.name}");

        if (other.gameObject.tag == "Player" && !_chestExpired)
        {
            Debug.Log("Trigger Entered....");
            if(_ChestType == ChestType.Blast)
            {
                if (other.gameObject.GetComponent<IntrantThirdPersonController>()._fightedKaire)
                {
                    Open();
                }
            }
            else
            {
                Open();
            }
        }
        else if (other.gameObject.tag != "Player")
        {
            try
            {
                var _rigidbody = GetComponent<Rigidbody>();
                Destroy(_rigidbody);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    private void FixedUpdate()
    {
        if (souvenirInstantiated)
        {
            if (souvenirCopy == null && lightFx.activeSelf)
            {
                lightFx.SetActive(false);
            }
        }
        if(GameManager.instance._mapName == MAPName.WEEK3)
        {
            if (puddle != null)
            {
                foreach(var obj in puddle)
                {
                    if(obj == null)
                    {
                        puddle.Remove(obj);
                    }
                }
                if (puddle.Count < 1)
                {
                    GameManager.instance._monsterEmerged  = true;
                    Destroy(gameObject, 0.2f);
                    Destroy(_puddlrainVfxRef, 0.2f);
                }
            }
        }
    }

    public void DestroyChest()
    {
        Debug.LogError("DestroyingChest");
        Destroy(GetComponent<Rigidbody>());
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 2f);
        Destroy(gameObject,2f);
    }
}


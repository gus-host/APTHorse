using System;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerStage : MonoBehaviour
{
    public static PlayerStage instance;

    public GameObject spOne;
    
    [Header(" Timeline Asset")]
    [SerializeField]
    public PlayableDirector _playableDirector;

    [SerializeField] private GameObject _stageVfx;
    [SerializeField] private GameObject _wallbreakSpellVFXMinimapPointer;

    [Header("Bool")]
    [SerializeField]
    public bool _playerIn = false;
    [SerializeField]
    public bool _playerOn = false;
    [SerializeField]
    public bool _playerOut = false;

    [SerializeField]
    public bool _wallBreakPotionCreated = false;

    [Header("Animations")]
    [SerializeField]
    private Animator _doorAnimation;

    [Header("Potion")]
    [SerializeField] public GameObject _potion;


    [Header("PotionSpawnPoints")]
    [SerializeField] public GameObject[] _spawnPoints;

    private void Start()
    {
        instance = this;
    }

    public void CreateWallBreakPotion()
    {
        if(!_wallBreakPotionCreated)
        {
            int index = UnityEngine.Random.Range(0,_spawnPoints.Length);
            GameObject potionWall = Instantiate(_potion, _spawnPoints[index].transform.position, Quaternion.identity);
            GameObject pointer = Instantiate(_wallbreakSpellVFXMinimapPointer);
            pointer.GetComponent<PlayerMinimapPointer>().target = potionWall.transform;
            potionWall.GetComponent<WallBreakSpell>()._pointer = pointer;
            _wallBreakPotionCreated = true;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.LogWarning("Player entered stage");
            _playerIn = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.LogWarning("Player on the stage");
            _playerOn = true;
            _playerOut = false;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.LogWarning("Player left stage");
            _playerOut = true;
            _playerOn = false;
            DisableStageVFX();
            //Play timeline
            if (_playableDirector != null)
            {
                _playableDirector.SetGenericBinding(_playableDirector.playableAsset, _stageVfx);
                _playableDirector.Play();
            }

            if (_doorAnimation != null)
            {
                _doorAnimation.SetTrigger("Close");
                Invoke("Disable", 1.5f);
            }
        }
    }

    private void DisableStageVFX()
    {
        _stageVfx.SetActive(false);
    }
}

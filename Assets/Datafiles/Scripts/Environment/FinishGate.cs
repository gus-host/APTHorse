using TMPro;
using UnityEngine;

public class FinishGate : MonoBehaviour
{
    public static FinishGate instance;

    public GameObject _portalVFX;


    public GameObject _gameOvercanvas;
    public TextMeshProUGUI _gameOvertxt;

    public EnemySpawner _enemySpawner;

    public int totalEnemies = 1;
    public bool _gameFinished = false;
    public bool _tryKairPass = false;
    
    private void Awake()
    {
        instance = this;
        IntrantThirdPersonController.CollectedAsimanaCodex += CheckForGameFinished;
    }

    private void LateUpdate()
    {
        if (!_gameFinished)
        {
            CheckForGameFinished();
        }   
    }

    private void OnDestroy()
    {
        IntrantThirdPersonController.CollectedAsimanaCodex -= CheckForGameFinished;
    }

    private void CheckForGameFinished()
    {
        MonsterMovementController[] _enemy = FindObjectsOfType<MonsterMovementController>();
        totalEnemies = _enemy.Length;
        
        if (totalEnemies <= 0)
        {
            _gameFinished = true;
            _portalVFX.SetActive(true);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Kaire")
        {
            Debug.LogError("Kaire Reached Finish line");
            _portalVFX.SetActive(false);
            Destroy(other.gameObject);
            _enemySpawner.SpawnEnemies();
            GiveAllEnemiesTarget();
        }
        if (other.gameObject.CompareTag(Tags.PLAYER_TAG) && _gameFinished)
        {
            SceneManager.instance.LoadScene("Week-2");
            SceneManager.instance._text.gameObject.SetActive(true);
            SceneManager.instance._text.text = "Level Completed";
            //_gameOvercanvas.SetActive(true);
            //_gameOvertxt.text = "Mission Complete!!";
            PlayerMovementManager.instance.gameObject.SetActive(false);
        }
    }

    private void GiveAllEnemiesTarget()
    {
        MonsterMovementController[] _enemies = FindObjectsOfType<MonsterMovementController>();
        IntrantThirdPersonController player = FindObjectOfType<IntrantThirdPersonController>();
        foreach (var enemy in _enemies)
        {
            enemy.target = player.transform;
            enemy.findEnemies = true;
            enemy._radiusRange = 50;
            enemy.SetState(MonsterState.CHASE);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Tags.Kaire_TAG))
        {
            Debug.LogError("Kaire Reached Finish line");
            _portalVFX.SetActive(false);
            Destroy(collision.gameObject);
        }
    }

}

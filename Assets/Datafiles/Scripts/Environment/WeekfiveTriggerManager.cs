using System.Collections;
using TMPro;
using UnityEngine;

public enum MapPoint
{
    None,
    MiraTarget,
    MirasFinalTarget,
    Maze,
    FinalPoint
}

public class WeekfiveTriggerManager : MonoBehaviour
{
    public MapPoint mapPoint;

    public bool AutoDamage = false;
    public bool isDamageCoroutineRunning = false;
    public float damageInterval = 5f;
    public string sceneName;
    public Animator _unlockAnimation;

    public GameObject _kair;
    public GameObject _mira;
    public GameObject _kairRef;
    public GameObject _bossFerraptor;
    public GameObject[] keys; 
    public GameObject[] tornados; 

    public enum DamageSate
    {
        Idle, Damaging
    }
    public DamageSate damageSate = DamageSate.Idle;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.PLAYER_TAG) && !isDamageCoroutineRunning && AutoDamage)
        {
            damageSate = DamageSate.Damaging;
            StartCoroutine(DamagePeriodically(other.gameObject));
        }else if (mapPoint == MapPoint.MiraTarget)
        {
            Vector3 _spawnPos = new Vector3(transform.position.x+ Random.Range(0,5), transform.position.y, transform.position.z + Random.Range(0,5));
            _kairRef = Instantiate(_kair, _spawnPos, Quaternion.identity);
            GetComponent<BoxCollider>().enabled = false;
        }else if(mapPoint == MapPoint.FinalPoint)
        {
            if (other.gameObject.TryGetComponent<IntrantThirdPersonController>( out IntrantThirdPersonController _player))
            {
                if (_player._collectedTherCode > 4)
                {

                    _player.GetComponent<PvPPlayerUI>()._collect.onClick.RemoveAllListeners();
                    _player.GetComponent<PvPPlayerUI>()._collect.gameObject.SetActive(true);
                    _player.GetComponent<PvPPlayerUI>()._collect.gameObject.GetComponentInChildren<TMP_Text>().text = "Place theracode and initiate";
                    _player.GetComponent<PvPPlayerUI>()._collect.onClick.AddListener(() =>
                    {
                        GetComponent<SceneManager>().enabled = true;
                        GetComponent<SceneManager>().LoadScene(sceneName);
                        foreach (var tornado in tornados)
                        {
                            tornado.SetActive(true);
                        }
                    });
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.PLAYER_TAG) && isDamageCoroutineRunning && AutoDamage)
        {
            damageSate = DamageSate.Idle;
            StopCoroutine(DamagePeriodically(other.gameObject));
            isDamageCoroutineRunning = false;
        }
    }

    private IEnumerator DamagePeriodically(GameObject player)
    {
        isDamageCoroutineRunning = true;

        while (true)
        {
            yield return new WaitForSeconds(damageInterval);

            if (damageSate == DamageSate.Damaging)
            {
                player.GetComponent<IntrantPlayerHealthManager>().DealDamage(0.3f);
            }
        }

        isDamageCoroutineRunning = false;
    }
}

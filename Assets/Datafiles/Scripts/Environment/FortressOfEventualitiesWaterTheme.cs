using System.Collections;
using UnityEngine;

public class FortressOfEventualitiesWaterTheme : MonoBehaviour
{
    [SerializeField] private ManualTag manualTag;
    [SerializeField] private GameObject []rainStonespawnPoints;

    [SerializeField] private GameObject _asimanaCodex;
    [SerializeField] private GameObject _rasveus;

    private void Start()
    {
        manualTag = GetComponent<ManualTag>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.PLAYER_TAG) && manualTag._tag == Tag.StoneFire)
        {
            foreach(var obj in rainStonespawnPoints)
            {
                obj.SetActive(true);
            }
        }
        if (other.gameObject.CompareTag(Tags.PLAYER_TAG) && manualTag._tag == Tag.Asimana_Flying && other.TryGetComponent<PlayerTimelineManager>(out PlayerTimelineManager _playerController))
        {
            GameObject _asimana = Instantiate(_asimanaCodex, other.transform.position, Quaternion.identity);
            _asimana.SetActive(false);
            LeanTween.scale(_asimana, new Vector3(0, 0, 0), 0.1f);
            _asimana.SetActive(true);
            LeanTween.scale(_asimana, new Vector3(0.05f, 0.05f, 0.05f), 2f);
            _asimana.AddComponent<MoveThings>().MoveUpAndDown(15f, 3f);
            Destroy(_asimana.GetComponent<RotateandLevitate>());
            Destroy(_asimana, 6f);
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(FlickerRasveus());
            _playerController.TheraFarAway();
        }
    }

    IEnumerator FlickerRasveus()
    {
        GameObject _obj = Instantiate(_rasveus,
            new Vector3(transform.position.x + (Random.Range(-3, 3)),
            transform.position.y,
            transform.position.z + (Random.Range(-3, 3))), Quaternion.Euler(-90,0,0));

        yield return new WaitForSeconds(0.1f);
        _obj.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        _obj.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        _obj.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        _obj.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        _obj.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        _obj.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _obj.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        _obj.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        _obj.SetActive(false);
        Destroy(_obj);
    }
}

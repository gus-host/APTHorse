using UnityEngine;

public class WeekFourTriggerManager : MonoBehaviour
{
    public enum MapPoint
    {
        ptTwo,
        DropletTarget,
        TheraSpawnPoint,
        AutomaticCaveSwitch,
        AsimanaInstruction,
        GatePlatform,
        DissolvingMagic,
        AutoCave,
        DropletTrailPoint
    }

    public MapPoint mapPoint;
    public GameObject _asimanaCodex;
    public GameObject _adacodeCryptex;
    public GameObject _glimmer;
    public GameObject _droplet;
    public GameObject _spts;
    public GameObject []_placedArtifacts;
    public GameObject _tornado;
    public GameObject []_sptsO;
    public Fracture []destructableRocks;

    public Animator _putTogetherAnimator;


    private void Start()
    {
        foreach (var rock in destructableRocks)
        {
            int randInt = Random.Range(0,4);
            if(randInt == 2)
            {
                rock.glimmerArtifact = _glimmer;
                _glimmer = null;
            }
        }
        if(_glimmer != null)
        {
            destructableRocks[destructableRocks.Length-1].glimmerArtifact = _glimmer;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.PLAYER_TAG) && mapPoint == MapPoint.AutomaticCaveSwitch)
        {
            GetComponent<Animator>().SetBool("TurnOn", true);
            GetComponent<BoxCollider>().enabled = false;
        }else if(other.gameObject.CompareTag(Tags.PLAYER_TAG) && mapPoint == MapPoint.AsimanaInstruction)
        {
            other.gameObject.GetComponent<PlayerTimelineManager>().PublicPauseAndResume(3);
            Vector3 _pos = new Vector3(transform.position.x + Random.Range(0,3), transform.position.y, transform.position.z + Random.Range(0,3));
            GameObject _asimana = Instantiate(_asimanaCodex, _pos, Quaternion.Euler(-42, 0, 90));
            _asimana.SetActive(false);
            LeanTween.scale(_asimana, new Vector3(0, 0, 0), 0f);
            _asimana.SetActive(true);
            if (_asimana.TryGetComponent<GeneralArtifactsManager>(out GeneralArtifactsManager _artifact))
            {
                if (_artifact.artifact == GeneralArtifactsManager.Artifact.AsimanaCodex && _artifact!=null)
                {
                    Debug.LogError("Turning on dialogue");
                    _artifact._asimanaDialogue.SetActive(true);
                }
            }
            LeanTween.scale(_asimana, new Vector3(0.05f, 0.05f, 0.05f), 2f);
            _asimana.AddComponent<MoveThings>().MoveUpAndDown(5f, 3f);
            Destroy(_asimana.GetComponent<RotateandLevitate>());
            Destroy(_asimana, 3f);

        }
        else if (other.gameObject.CompareTag(Tags.PLAYER_TAG) && mapPoint != MapPoint.AutomaticCaveSwitch && mapPoint != MapPoint.AsimanaInstruction && mapPoint != MapPoint.GatePlatform && _spts != null)
        {
            GameObject _asimanaCodexRef = Instantiate(_asimanaCodex, _spts.transform.position, Quaternion.Euler(-90, 0, 90));
            _asimanaCodexRef.AddComponent<Rigidbody>();
            _asimanaCodexRef.GetComponent<RotateandLevitate>().enabled = false;
            _asimanaCodexRef.GetComponent<BoxCollider>().isTrigger = false;

            for (int i = 0; i < 4; i++)
            {
                GameObject _adacodeCryptexRefO = Instantiate(_adacodeCryptex, _sptsO[i].transform.position, Quaternion.identity);
            }
        }
    }

    public void PlacedArtifacts()
    {
        foreach (var artifact in _placedArtifacts)
        {
            artifact.SetActive(true);
        }
        _tornado.SetActive(true);
    }
}
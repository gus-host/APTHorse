using UnityEngine;

public class GeneralArtifactsManager : MonoBehaviour
{
    public enum Artifact
    {
        MagicalKey,
        AsimanaCodex,    
        MeronNftPartOne,
        MeronNftPartTwo,
        MeronNftPartThree,
        MeronNft,
        GlimmerArtifact,
        TheraCode,
        HealthPotion,
        PowerPotion
    }
    public Artifact artifact;

    public GameObject _meronNFT;
    public GameObject _asimanaDialogue;
    public GameObject []_glimmerCenter;

    public bool _nftInstantiated;

    public int glimmerType = 0;
    private void OnEnable()
    {
        if (_asimanaDialogue != null)
        {
            _asimanaDialogue.SetActive(false);
        }
    }

    private void Awake()
    {
        
    }

    private void Start()
    {
        if(artifact == Artifact.GlimmerArtifact)
        {
            glimmerType = Random.Range(0,4);
            //glimmerType = 1;
            if(glimmerType == 0)
            {
               _glimmerCenter[glimmerType].SetActive(true);
            }else if (glimmerType == 1)
            {
               _glimmerCenter[glimmerType].SetActive(true);
            }
            else if (glimmerType == 2)
            {
               _glimmerCenter[glimmerType].SetActive(true);
            }
            else if (glimmerType == 3)
            {
               _glimmerCenter[glimmerType].SetActive(true);
            }
            LeanTween.scale(gameObject, new Vector3(150, 150, 150), 1);
            LeanTween.move(gameObject, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), 1);
        }
        else if (artifact == Artifact.TheraCode)
        {
            Invoke("FreezeTheraCode", 3f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (artifact == Artifact.AsimanaCodex)
        {
            if (collision.gameObject.CompareTag("Floor"))
            {
                if(!_nftInstantiated)
                {
                    _nftInstantiated = true;
                    GetComponent<Rigidbody>().isKinematic = true;
                    GameObject _nft = Instantiate(_meronNFT, transform.position, Quaternion.identity);
                    LeanTween.scale(_nft, new Vector3(0, 0, 0), 0);
                    LeanTween.scale(_nft, new Vector3(50, 50, 5), 1);
                    LeanTween.move(_nft, new Vector3(_nft.transform.position.x, _nft.transform.position.y + 1.5f, _nft.transform.position.z), 1);
                }
            }
            if (collision.gameObject.CompareTag(Tags.PLAYER_TAG))
            {
                LeanTween.scale(gameObject, new Vector3(0, 0, 0), 1);
                Destroy(gameObject, 1);
            }
        }

    }

/*    void FreezeTheraCode()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }*/
}

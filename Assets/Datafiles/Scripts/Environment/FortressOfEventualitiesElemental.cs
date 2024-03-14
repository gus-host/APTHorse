using UnityEngine;

public class FortressOfEventualitiesElemental : MonoBehaviour
{
    public bool stoneCourtyard = false;
    public bool disintegrated = false;
    public bool integrated = false;
    public bool tornadoPoint = false;

    [Header("Orin")]
    public GameObject _orin;
    public GameObject _orinSpts;
    public GameObject _orinMagicWall;

    public GameObject _teleportChamber;
    public MeshRenderer []_mesh;
    public Material _wallMat;
    public Material _floorMat;
    public Material _transWallMat;
    public Material _transFloorMat;
    public GameObject blastFx;
    public GameObject _orinSpawnPoints;

    public GameObject []tornado;
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag(MapPointsWeekThree.SimpleClaw) || other.gameObject.CompareTag(MapPointsWeekThree.PlayersClaw) || other.gameObject.CompareTag(MapPointsWeekThree.PlayersClaw))
        {
            if (stoneCourtyard)
            {
                Debug.LogError("Disintegrated");
                Disintegrate();
            }
            else if (tornadoPoint)
            {
                foreach (var _obj in tornado)
                {
                    _obj.SetActive(true);
                }
                Invoke("LoadNextMap", 5f);
            }
        }else if (other.gameObject.CompareTag(Tags.PLAYER_TAG) && other.gameObject.TryGetComponent<IntrantThirdPersonController>(out IntrantThirdPersonController playerController))
        {
            if (_teleportChamber != null)
            {
                _teleportChamber.GetComponent<Animator>().enabled = true;
            }
            else
            {
                if (playerController._fixedStoneCourtyard == true)
                {
                    GameObject orin = Instantiate(_orin, _orinSpts.transform.position, Quaternion.identity);
                    _orinMagicWall.SetActive(true);
                    Destroy(orin, 30f);
                }
            }
        }
    }

    private void  LoadNextMap()
    {
        SceneManager.instance.LoadScene("Week-4");
        SceneManager.instance._text.gameObject.SetActive(true);
        SceneManager.instance._text.text = "Level Completed";
    }
    public void Disintegrate()
    {
        if(disintegrated)
        {
            Debug.LogError("Disintegrated already");
            return;
        }
        foreach (MeshRenderer obj in _mesh)
        {
            Material[] _materials = obj.materials;
            int index = 0;
            foreach (Material mat in _materials)
            {
                string tempFloofMat = "";
                string tempWallMat = "";
                if (_floorMat != null)
                {
                    tempFloofMat = _floorMat.name + " (Instance)";
                }
                if (_wallMat != null)
                {
                    tempWallMat = _wallMat.name + " (Instance)";
                }

                if (mat.name == tempFloofMat && _transFloorMat != null)
                {
                    _materials[index] = _transFloorMat;
                    Debug.LogError("Disintegrated Changed material");
                }
                else if (mat.name == tempWallMat && _transWallMat != null)
                {
                    _materials[index] = _transWallMat;
                    Debug.LogError("Disintegrated Changed material");
                }
                ++index;
            }
            obj.materials = _materials;
        }
        int total = 5;
        while (total>0)
        {
            Instantiate(blastFx, transform.position, Quaternion.identity);
            total--;
        }
        disintegrated = true;
        PlayerTimelineManager _player = FindObjectOfType<PlayerTimelineManager>();
        if (_player != null)
        {
            _player.RepairDisintegrated();
            _player.GetComponent<GrimoireManager>()._hologramBinary.interactable = true;
        }
    }

    public void Integrate()
    {
        foreach (var obj in _mesh)
        {
            Material[] _materials = obj.materials;
            int index = 0;
            foreach (Material mat in _materials)
            {
                string tempRoofMat = "";
                string tempWallMat = "";
                if (_transFloorMat != null)
                {
                    tempRoofMat = _transFloorMat.name + " (Instance)";
                }
                if (_transWallMat != null)
                {
                    tempWallMat = _transWallMat.name + " (Instance)";
                }

                if (mat.name == tempRoofMat && _floorMat != null)
                {
                    _materials[index] = _floorMat;
                }
                else if (mat.name == tempWallMat && _wallMat != null)
                {
                    _materials[index] = _wallMat;
                }
                ++index;
            }
            obj.materials = _materials;
        }
        gameObject.SetActive(false);

    }
}

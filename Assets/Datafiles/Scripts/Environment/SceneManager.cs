using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;
    //GameObjects
    [SerializeField] private GameObject _loadingScreen;

    //UI
    [SerializeField] private Image _fill;
    [SerializeField] public TMP_Text _text;

    //SceneName to be loaded 
    [Header("Scene Name To Be Loaded")]
    [SerializeField] private string _sceneName;

    //Enable or Disable if want to start the scene loading at the beginning of the game

    [Header("Load Scene on Start")]
    [SerializeField] private bool _initOnStart = false;
    [SerializeField] private bool _loading = false;
    [SerializeField] private bool _onTriggerLoad = false;
    
    private void Start()
    {
        instance = this;
        if (_initOnStart)
        {
            if (!string.IsNullOrEmpty(_sceneName))
            {
                LoadScene(_sceneName);
                Debug.LogError("Initiating scene Load....");
            }
            else
            {
                Debug.LogError("Please enter scene name");
            }
        }

        PhotonNetwork.JoinLobby();
    }
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
        Debug.LogWarning("LoadingScene..");
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        Debug.LogError($"SceneName {sceneName}");
        if (_loading || _loadingScreen.activeSelf)
            yield return null;
        _loading = true;
        yield return new WaitForSeconds(2.5f);
        if (_loadingScreen)
        {
            _loadingScreen.SetActive(true);
        }
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            // You can display a loading progress bar or perform other tasks here if needed
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            if (_fill != null)
            {
                _fill.fillAmount = progress;
            }
            Debug.LogError("Loading progress: " + progress * 100 + "%");
            
            yield return null;
            _loading = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_onTriggerLoad && other.gameObject.CompareTag(Tags.PLAYER_TAG) && other.gameObject.TryGetComponent<IntrantThirdPersonController>(out IntrantThirdPersonController _player) )
        {
            if (_player._foundThrowingStar && _player._map ==MAP.WEEK5)
            {
                LoadScene(_sceneName);
            }
            else if (_player._map == MAP.WEEK4)
            {
                LoadScene(_sceneName);
            }
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class UIContainer :MonoBehaviour
{
    [SerializeField] public Button []_newGameBtn;
    [SerializeField] public Button []_mainMenu;
    [SerializeField] public Button _backbtn;
    [SerializeField] public Button _resumeBtn;
    [SerializeField] public Button _quitBtn;
    [SerializeField] public Button _settingBtn;
    [SerializeField] public Button _pauseAndPlay;
    [SerializeField] public Button _mapBtn;
    [SerializeField] public GameObject _settingPanel;
    [SerializeField] public string ResumeSceneToLoad;
    [SerializeField] public GameObject _loadingScreen;
    [SerializeField] public Image _fill;
    [SerializeField] public Sprite _pause;
    [SerializeField] public Sprite _play;
}

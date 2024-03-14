using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : UIContainer
{
    public static UI_Manager Instance;

    public bool pauseStat = false;

    private void Start()
    {
        Instance = this;
        foreach (var t in _newGameBtn)
        {
            t.onClick.AddListener(NewGame);
        }
        foreach (var t in _mainMenu)
        {
            t.onClick.AddListener(MainMenu);
        }
        _quitBtn.onClick.AddListener(QuitGame);
        _resumeBtn.onClick.AddListener(ResumeGame);
        _backbtn.onClick.AddListener(ResumeGame);
        _settingBtn.onClick.AddListener(Setting);
        _pauseAndPlay.onClick.AddListener(PauseAndPlay);
    }

    private void PauseAndPlay()
    {
        if(pauseStat)
        {
            _pauseAndPlay.GetComponent<Image>().sprite = _pause;
            Time.timeScale = 1.0f; 
            pauseStat = false;
        }
        else if(!pauseStat)
        {
            _pauseAndPlay.GetComponent<Image>().sprite = _play;
            Time.timeScale = 0f;
            pauseStat = true;
        }
    }

    private void NewGame()
    {
        Time.timeScale = 1;
        _loadingScreen.SetActive(true);
        IntrantThirdPersonController.instance.Reload(UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex).name, _loadingScreen, _fill);
    }

    private void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.instance.LoadScene("MainMenu");
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        _settingPanel.SetActive(false);
    }

    private void Setting()
    {
        _settingPanel.SetActive(true);
        Time.timeScale = 0;
    }
    
    private void QuitGame()
    {
        Application.Quit();
    }

}
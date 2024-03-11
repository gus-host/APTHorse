using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PvPSceneManager : MonoBehaviourPunCallbacks
{
    public static PvPSceneManager instance;
    public Button _surrender;
    public TMP_Text toastTextPrefab;
    public GameObject _content;
    public GameObject content;

    private void Start()
    {
        _surrender.onClick.AddListener(Surrender);
        instance = this;
    }

    private void Surrender()
    {
        PhotonNetwork.LeaveRoom();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        StartCoroutine(ShowToastCoroutine(otherPlayer, false));
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        StartCoroutine(ShowToastCoroutine(newPlayer, true));
    }

    IEnumerator ShowToastCoroutine(Player player, bool val)
    {
        // Set the toast message
        var toastText = Instantiate(toastTextPrefab.gameObject, content.transform);
        TMP_Text _toast = toastText.GetComponent<TMP_Text>();
        if(!val)
        {
            _toast.color = Color.red;
            _toast.text = player.NickName+" left";
        }
        else
        {
            _toast.color = Color.green;
            _toast.text = player.NickName+" joined";
        }

        // Fade in the toast
        _toast.CrossFadeAlpha(1f, 0.5f, false);

        // Wait for the specified duration
        yield return new WaitForSeconds(2);

        // Fade out the toast
        _toast.CrossFadeAlpha(0f, 0.45f, false);
        yield return new WaitForSeconds(0.45f);
        Destroy(toastText, 1f);
    }
}

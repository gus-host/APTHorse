using TMPro;
using UnityEngine;

public class SpinnerManager : MonoBehaviour
{
    public GameObject spinnerHolder;
    public TextMeshProUGUI message;

    public void ShowMessage(string msg)
    {
        message.text = msg;
        spinnerHolder.SetActive(true);
    }

    public void HideMessage()
    {
        spinnerHolder.SetActive(false);
    }
}
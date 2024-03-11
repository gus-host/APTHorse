using TMPro;
using UnityEngine;

public class CustomToast : MonoBehaviour
{
    public CanvasGroup toast;
    public TMP_Text _text;
    public void ShowToast()
    {
        toast.alpha = 1;
        LeanTween.value(1, 0, 4).setOnUpdate((float val) => toast.alpha = val);
    }
}

using UnityEngine;

public class ComingSoon : MonoBehaviour
{
    public CanvasGroup comingSoonPanel;

    public void ShowComingSoon()
    {
        comingSoonPanel.alpha = 1;
        LeanTween.value(1, 0, 2).setOnUpdate((float val) => comingSoonPanel.alpha = val);
    }
}

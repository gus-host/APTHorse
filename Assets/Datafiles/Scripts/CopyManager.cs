using TMPro;
using UnityEngine;

public class CopyManager : MonoBehaviour
{
    public void CopyFromText(TextMeshProUGUI val)
    {
        GUIUtility.systemCopyBuffer = val.text;
    }
}

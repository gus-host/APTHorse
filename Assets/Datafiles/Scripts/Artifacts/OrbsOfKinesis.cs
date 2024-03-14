using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OrbsOfKinesis : MonoBehaviour
{
    public static OrbsOfKinesis instance;
    public GameObject _stableFire;
    public GameObject _RotateryFire;
    public GameObject rock;
    public GameObject orb;

    private void Start()
    {
        instance = this;
    }
    
    public void RemoveMagic(Button adacodeCryptex, GameObject _grimoire)
    {
        _stableFire.SetActive(false);
        _RotateryFire.SetActive(true);
        StartCoroutine(RemoveMagicAndEnd(adacodeCryptex, _grimoire));
        TryGetComponent<ButtonPingPongAnimations>(out ButtonPingPongAnimations pingpongAnim);
        if (pingpongAnim != null)
        {
            Destroy(pingpongAnim);
        }
    }
    
    IEnumerator RemoveMagicAndEnd(Button adacodeCryptex, GameObject _grimoire)
    {
        DisablePanel(_grimoire);
        yield return new WaitForSeconds(7f);
        _RotateryFire.SetActive(false);
        Destroy(_stableFire);
        Destroy(_RotateryFire);
        GetComponent<RotateandLevitate>().enabled = false;
        adacodeCryptex.interactable = false;
    }
    public void DisablePanel(GameObject grimoire)
    {
        Time.timeScale = 1f;
        grimoire.SetActive(false);
    }
}

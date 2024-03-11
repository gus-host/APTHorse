using System;
using UnityEngine;

public class EmitterOrbsManager : MonoBehaviour
{
    public static EmitterOrbsManager instance;
    public GameObject _portal;
    public GameObject _kaire;
    public GameObject _kaireRef;
    public GameObject _sp;

    private void Start()
    {
        instance = this;
    }

    public void TurToEmitters()
    {
        GetComponent<Animator>().SetTrigger("TurnToEmitter");
        _portal.SetActive(true);
        GameObject kaire = Instantiate(_kaire, _sp.transform.position, Quaternion.identity);
        _kaireRef = kaire;
        GetComponent<EnemySpawner>().enabled = true;
        GetComponent<BoxCollider>().enabled = false;
        Invoke("Disrupt", 1f);
    }

    private void Disrupt()
    {
        GetComponent<Animator>().SetTrigger("Disrupt");
    }

    public void ReInitiate()
    {
        _kaireRef.GetComponent<EnemyController>()._headingToPortal = true;
        Invoke("DisableMisc",1f);
        //GetComponent<Animator>().SetTrigger("TurnToEmitter");
        GetComponent<BoxCollider>().enabled = false;
        IntrantThirdPersonController.instance.CollectRemainingOrb();
    }

    private void DisableMisc()
    {
        _portal.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.PLAYER_TAG))
        {
            GetComponent<BoxCollider>().enabled = false;
        }      
    }
}

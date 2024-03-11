using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnatchManager : MonoBehaviour
{
    public Animator _anim;
    public GoblinManager _goblinManager;
    public GoblinAnimationController _animController;

    [Header("Bools")]
    public GameObject _playerRef;

    [Header("Bools")]
    public bool _inRange = false;
    public bool _snatched = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _animController.Snatch();
            _inRange = true;
            Snatch(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _inRange = false;
            //_animController.Run(true);
        }
    }

    private void Snatch(GameObject _obj)
    {
        _playerRef = _obj;
        Invoke("ToggleSnatch", 1.3f);
        _goblinManager._snatched = true;
    }
    public void ToggleSnatch()
    {
        _snatched = true;
    }
}
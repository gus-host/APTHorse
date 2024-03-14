using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashableWall : MonoBehaviour
{
    public static SmashableWall _instance; 
    [SerializeField] public GameObject []breakableWalls;

    [Header("Wall")]
    public MeshCollider _wall;

    [Header("Bools")]
    public bool _smashed = false;
    public bool _canSmash = false;

    [Header("Lantern")]
    public Rigidbody[] _lantern; 

    private void Start()
    {
        _instance = this;
    }

    private void Update()
    {
        if (!_smashed && _canSmash)
        {
            try
            {
                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<BoxCollider>().enabled = false;
                GetComponent<MeshCollider>().enabled = false;
            }
            catch(Exception e)
            {
                
            }
            foreach (var c in breakableWalls)
            {
                c.SetActive(true);
                c.GetComponent<Fracture>().FractureObject();
            }
            foreach(Rigidbody _l in _lantern)
            {
                _l.constraints = RigidbodyConstraints.None;
            }
            PvPPlayerUI.instance._wallBreakbtn.gameObject.SetActive(false);
            Destroy(_wall.gameObject,0.2f);
            _smashed = true;
            _canSmash = false;
        }
    }
}
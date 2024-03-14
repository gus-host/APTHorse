using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HenchmenType
{
    Milut
}

public class Henchmen : MonoBehaviour
{
    public Animator _animator;


    private void Start()
    {
        _animator = GetComponent<Animator>();
    }


}

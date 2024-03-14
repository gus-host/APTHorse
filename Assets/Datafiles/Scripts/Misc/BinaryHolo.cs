using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryHolo : MonoBehaviour
{
    public GameObject _seqInit;

    public void Initiate()
    {
        _seqInit.SetActive(true);
    }
}

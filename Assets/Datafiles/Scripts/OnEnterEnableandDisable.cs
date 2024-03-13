using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterEnableandDisable : MonoBehaviour
{
    public GameObject _obj;
    public GameObject []_objts;
    public bool _enabled;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && _obj!=null)
        {
            _obj.SetActive(_enabled);
        }
        if(collision.gameObject.CompareTag("Player") && _objts.Length >0)
        {
            foreach (var obj in _objts)
            {
                obj.SetActive(_enabled);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinMole : MonoBehaviour
{
    public static GoblinMole _instance;
    public List<GameObject> _mole= new List<GameObject> ();
    // Start is called before the first frame update

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : MonoBehaviour
{
    public GameObject[] items;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Initialize()
    {
        int itemsToUse = Random.Range(0, items.Length);
        GameObject item = Instantiate(items[itemsToUse], transform.position, Quaternion.identity);
        item.transform.parent = this.transform;
        item.name = this.gameObject.name;
    }
}

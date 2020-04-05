using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("destroyGameObject");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator destroyGameObject()
    {
        yield return new WaitForSeconds(4.2f);
        Destroy(gameObject);
    }
}

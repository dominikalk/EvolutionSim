using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    Animator myAnim;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void eat()
    {
        StartCoroutine("eatCo");
    }

    IEnumerator eatCo()
    {
        myAnim.SetTrigger("eat");
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    Animator myAnim;
    GameObject mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        //StartCoroutine("checkCamera");
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

    IEnumerator checkCamera()
    {
        while (true)
        {
            if(Vector3.Distance(transform.position, mainCamera.transform.position) > 40)
            {
                MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
                foreach(MeshRenderer mesh in meshRenderers)
                {
                    mesh.enabled = false;
                }
            }
            else
            {
                MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer mesh in meshRenderers)
                {
                    mesh.enabled = true;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}

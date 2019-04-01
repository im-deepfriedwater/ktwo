using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAtEndOfLifeTime : MonoBehaviour 
{
    public float lifetime;

    // Use this for initialization
    void Start()
    {
        StartCoroutine("BeginCountDown");
    }

    IEnumerator BeginCountDown ()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}

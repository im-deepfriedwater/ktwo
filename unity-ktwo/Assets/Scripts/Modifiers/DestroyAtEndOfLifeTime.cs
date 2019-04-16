using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
        NetworkServer.Destroy(gameObject.transform.parent.gameObject);
    }
}

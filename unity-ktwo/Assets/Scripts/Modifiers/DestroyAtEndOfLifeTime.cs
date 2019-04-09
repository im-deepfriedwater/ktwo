using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DestroyAtEndOfLifeTime : MonoBehaviour {
    public float lifetime;
    float currentTimePassed = 0f;

    // Use this for initialization
    void Start()
    {
        StartCoroutine("BeginCountDown");
    }

    IEnumerator BeginCountDown ()
    {
        while (currentTimePassed < lifetime)
        {
            currentTimePassed += Time.deltaTime;
            yield return null;
        }
        NetworkServer.Destroy(gameObject.transform.parent.gameObject);
    }


}

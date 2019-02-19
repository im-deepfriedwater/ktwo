using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var pos = GetComponent<Transform>().position;
        GetComponent<Transform>().position = new Vector3(pos.x, pos.y + pos.y *0.01f * Time.deltaTime, pos.z);
    }
}

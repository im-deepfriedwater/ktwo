using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DELETETHIS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(100, 0, 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

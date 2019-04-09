using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DeleteThisAfter : NetworkBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (!isServer)
        {
            Debug.Log("DEBUG S K I P");
            return;
        }
        transform.position = new Vector3(transform.position.x + .001f, transform.position.y, transform.position.z);
    }
}

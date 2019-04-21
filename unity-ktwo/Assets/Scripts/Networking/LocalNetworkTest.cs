using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LocalNetworkTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<NetworkManager>().StartServer();
    }
}

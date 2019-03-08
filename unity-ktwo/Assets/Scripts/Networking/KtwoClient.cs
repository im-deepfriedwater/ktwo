using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class KtwoClient : MonoBehaviour
{
    NetworkManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<NetworkManager>();
        manager.StartClient();
    }
}

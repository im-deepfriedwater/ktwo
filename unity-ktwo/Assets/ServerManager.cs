using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerManager : MonoBehaviour
{
    KtwoServer server;
    // Start is called before the first frame update
    void Start()
    {
        server = GetComponent<KtwoServer>();
        server.StartServer();
    }
}

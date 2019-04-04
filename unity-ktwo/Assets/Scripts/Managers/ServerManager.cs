using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerManager : NetworkBehaviour
{
    KtwoServer server;
    public static ServerManager instance;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        server = GetComponent<KtwoServer>();
        server.StartServer();
    }
}

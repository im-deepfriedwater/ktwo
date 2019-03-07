using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerManager : MonoBehaviour
{
    NetworkManagerHost managerHost;
    // Start is called before the first frame update
    void Start()
    {
        managerHost = GetComponent<NetworkManagerHost>();
        managerHost.StartServer();
    }
}

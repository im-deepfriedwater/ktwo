using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientGUI : MonoBehaviour
{
    public InputField ipAddress;
    public InputField port;

    public void ClientConnect()
    {
        KtwoClient.instance.networkAddress = ipAddress.text.Trim();
        KtwoClient.instance.networkPort = int.Parse(port.text);
        KtwoClient.instance.StartClient();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestServerReload : MonoBehaviour
{
    public void RestartServerScene()
    {
        KtwoServer.instance.StopServer();
        SceneManager.LoadScene("ServerEncounter");
    }
}

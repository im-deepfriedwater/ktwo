using UnityEngine.Networking;

public class PlayerSpawner : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!isServer)
        {
            return;
        }
        SpawnManager.instance.playerSpawns.Add(this.gameObject);
    }
}

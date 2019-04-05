using UnityEngine.Networking;

// Attaching this script to an object makes 
// that object a potential zombie spawner.
public class ZombieSpawner : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!isServer)
        {
            return;
        }
        SpawnManager.instance.zombieSpawns.Add(this.gameObject);
    }
}

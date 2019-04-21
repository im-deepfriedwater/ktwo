using UnityEngine;

// Attaching this script to an object makes 
// that object a potential zombie spawner.
public class ZombieSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SpawnManager.instance)
        {
            SpawnManager.instance.zombieSpawns.Add(this.gameObject);
        }

        Destroy(GetComponent<MeshRenderer>());
    }
}

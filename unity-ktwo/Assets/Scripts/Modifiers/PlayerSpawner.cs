using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SpawnManager.instance)
        {
            SpawnManager.instance.playerSpawns.Add(this.gameObject);
        }

        Destroy(GetComponent<MeshRenderer>());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuild : AbstractBehaviour
{
    [SerializeField]
    public GameObject wallPrefab; // Wall prefab that the Architect builds.
    [SerializeField]
    public float wallBuildDistance = 5; // Distance to spawn wall from player.
    [SerializeField]
    float wallHeight = 5;

    Transform playerTransform;

    private void Start()
    {
        playerTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (inputState.GetButtonValue(inputButtons[0]) && cooldownOver) 
        {
            cooldownOver = false;
            StartCoroutine("WaitForCooldown");
            SpawnWall();
        }
    }

    void SpawnWall()
    { 
        var newPosition = new Vector3(
            playerTransform.position.x, 
            playerTransform.position.y + wallHeight, 
            playerTransform.position.z + wallBuildDistance
        );
   
        Object.Instantiate(wallPrefab, newPosition, playerTransform.rotation);
    }

}

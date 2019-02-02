using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuild : AbstractAbility
{
    [SerializeField]
    GameObject wallPrefab; // Wall prefab that the Architect builds.
    [SerializeField]
    float wallDistanceFactor = 3.5f; // Distance to spawn wall from player.
    [SerializeField]
    float wallHeightOffset = 5;

    Transform playerTransform;

     void Start()
    {
        Initialize();
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
        UpdateAbilityUI();
    }

    void SpawnWall()
    {
        var newPosition = new Vector3(
            playerTransform.position.x,
            playerTransform.position.y + wallHeightOffset,
            playerTransform.position.z
        );

        newPosition += playerTransform.forward * wallDistanceFactor;
        Instantiate(wallPrefab, newPosition, playerTransform.rotation);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;

public class PlayerBehaviour : MonoBehaviour
{
    public float defaultSpeed;
    vThirdPersonController playerController;
    Collider lastCollided;

    [Range(0, 10)]
    public float boostedSpeedFactor; // 0 - 1

    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<vThirdPersonController>();
        ResetSpeed();
    }


    void Update()
    {
        Debug.Log("lol");
        GetComponent<Rigidbody>().AddForce(100, 100, 100, ForceMode.Force);
        // if (!lastCollided) // Continegency in case trigger is destroyed before OnExit is called
        // {
        //     ResetSpeed();
        // }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SpeedBoost")
        {
            lastCollided = other;
            playerController.freeRunningSpeed = defaultSpeed * (1 + boostedSpeedFactor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "SpeedBoost")
        {
            ResetSpeed();
        }
    }

    private void ResetSpeed()
    {
        playerController.freeRunningSpeed = defaultSpeed;
    }
}

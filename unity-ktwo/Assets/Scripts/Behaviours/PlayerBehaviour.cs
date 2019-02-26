using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;
using UnityEngine.UI;

// For controlling the main behaviours of the player.
public class PlayerBehaviour : MonoBehaviour
{   
    public Slider healthBar;
    public float defaultSpeed;
    [Range(0, 10)]
    public float boostedSpeedFactor; // Goes from 0 - 1.
    public bool recentlyHit;

    vThirdPersonController playerController;
    Collider lastCollided;
    
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<vThirdPersonController>();
        healthBar = GameObject.Find("HealthBarSlider").GetComponent<Slider>();
        ResetSpeed();
    }

    void Update()
    {
        if (!lastCollided) // Continegency in case trigger is destroyed before OnExit is called
        {
            ResetSpeed();
        }
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

    public void UpdateHealthBar(float healthPercentage)
    {
        healthBar.value = 1 - healthPercentage;
    }

    public void Die ()
    {
        Debug.Log("ur ded");
    }
}

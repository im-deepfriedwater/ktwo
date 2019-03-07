using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;
using UnityEngine.UI;
using UnityEngine.Networking;

// For controlling the main behaviours of the player.
public class PlayerBehaviour : NetworkBehaviour
{   
    [HideInInspector]
    Slider healthBar;
    [Tooltip("UI for game screen. Should be in Canvas, called GameOverScreen")]
    public GameObject gameOverScreen;
    public float defaultSpeed;
    [Range(0, 10)]
    public float boostedSpeedFactor; // Goes from 0 - 1.
    public bool recentlyHit;

    vThirdPersonController playerController;
    vThirdPersonInput input;
    Collider lastCollided;
    Animator animator;
    Rigidbody rbd;
    
    void Start()
    {
        playerController = GetComponent<vThirdPersonController>();
        healthBar = GameObject.Find("HealthBarSlider").GetComponent<Slider>();
        input = GetComponent<vThirdPersonInput>();
        animator = GetComponentInChildren<Animator>();
        rbd = GetComponent<Rigidbody>();
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
        InitiateGameOver();
    }

    public void InitiateGameOver()
    {
        gameOverScreen.SetActive(true);
        input.enabled = false;
        animator.SetBool("IsDead", true);
        rbd.isKinematic = true;
    }
}

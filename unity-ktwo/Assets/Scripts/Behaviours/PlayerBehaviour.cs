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
    public bool recentlyHit;
    public bool isDead;

    vThirdPersonController playerController;
    vThirdPersonInput input;
    Collider lastCollided;
    Animator animator;
    Rigidbody rbd;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rbd = GetComponent<Rigidbody>();

        if (isServer)
        {
            PlayerManager.instance.players.Add(gameObject);
        }

        // If this represents a different client's player,
        // We will shut off a lot of components it doesn't
        // need to compute and return.
        if (!hasAuthority)
        {
            TurnOffComponentsForNonLocalClient();
            return;
        }

        PlayerManager.instance.player = gameObject;
        playerController = GetComponent<vThirdPersonController>();
        healthBar = GameObject.Find("HealthBarSlider").GetComponent<Slider>();
        input = GetComponent<vThirdPersonInput>();
        InputManager.instance.Initialize(gameObject);
        PlayerManager.instance.players.Add(gameObject);
        ResetSpeed();
    }

    public void TurnOffComponentsForNonLocalClient()
    {
        Destroy(GetComponent<vThirdPersonController>());
        Destroy(GetComponent<vThirdPersonInput>());
    }

    public void AffectSpeed(float percent, bool buff)
    {
        if (!hasAuthority) return;

        var speedChange = defaultSpeed * percent;
        playerController.freeRunningSpeed = buff ? (defaultSpeed + speedChange) : (defaultSpeed - speedChange);
    }

    [ClientRpc]
    public void RpcTimedAffectSpeed(float percent, float time, bool buff)
    {
        if (!hasAuthority) return;
        StartCoroutine(TimedAffectSpeedCR(percent, time, buff));
    }

    public IEnumerator TimedAffectSpeedCR(float percent, float time, bool buff)
    {
        var speedChange = defaultSpeed * percent;
        playerController.freeRunningSpeed = buff ? (defaultSpeed + speedChange) : (defaultSpeed - speedChange);
        yield return new WaitForSeconds(time);
        ResetSpeed();
    }

    public void ResetSpeed()
    {
        if (!hasAuthority) return;
        playerController.freeRunningSpeed = defaultSpeed;
    }

    public void UpdateHealthBar(float healthPercentage)
    {
        if (!hasAuthority) return;
        healthBar.value = 1 - healthPercentage;
    }

    public void Die()
    {
        animator.SetBool("IsDead", true);

        if (!hasAuthority) return;

        input.enabled = false;
        rbd.isKinematic = true;
        isDead = true;

        if (isServer)
        {
            Debug.Log(PlayerManager.instance.players.Remove(gameObject) == false);
        }
        InitiateGameOver();
    }

    public void InitiateGameOver()
    {
        if (!hasAuthority) return;
        // gameOverScreen.SetActive(true);
    }
}

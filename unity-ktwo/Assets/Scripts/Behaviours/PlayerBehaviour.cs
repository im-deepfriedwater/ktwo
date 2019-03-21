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

    vThirdPersonController playerController;
    vThirdPersonInput input;
    Collider lastCollided;
    Animator animator;
    Rigidbody rbd;
    
    void Start()
    {

        // If this represents a different client's player,
        // We will shut off a lot of components it doesn't
        // need to compute and return.
        if (!hasAuthority)
        {
            TurnOffComponentsForNonLocalClient();
            return;
        }
    
        playerController = GetComponent<vThirdPersonController>();
        healthBar = GameObject.Find("HealthBarSlider").GetComponent<Slider>();
        input = GetComponent<vThirdPersonInput>();
        animator = GetComponentInChildren<Animator>();
        rbd = GetComponent<Rigidbody>();
        InputManager.instance.Initialize(this.gameObject);
        ResetSpeed();
    }

    public void TurnOffComponentsForNonLocalClient()
    {
        Destroy(GetComponent<Rigidbody>());
    }

    public void AffectSpeed(float percent, bool buff)
    {
        var speedChange = defaultSpeed * percent;
        playerController.freeRunningSpeed = buff ? (defaultSpeed + speedChange) : (defaultSpeed - speedChange);
    }

    public IEnumerator TimedAffectSpeed(float percent, float time, bool buff, HashSet<GameObject> set = null) 
    {
        var speedChange = defaultSpeed * percent;
        playerController.freeRunningSpeed = buff ? (defaultSpeed + speedChange) : (defaultSpeed - speedChange);
        yield return new WaitForSeconds(time);
        if (set != null) set.Remove(gameObject);
        ResetSpeed();
    }

    public void ResetSpeed()
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

    [Command]
    public void CmdBuildObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        Debug.Log("????????????");
        var go = Instantiate(prefab, position, rotation);
        NetworkServer.Spawn(go);
        go.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
    }
}

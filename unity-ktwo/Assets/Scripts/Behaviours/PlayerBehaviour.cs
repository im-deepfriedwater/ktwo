using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;
using UnityEngine.UI;

// For controlling the main behaviours of the player.
public class PlayerBehaviour : MonoBehaviour
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
        playerController = GameObject.Find("Player").GetComponent<vThirdPersonController>();
        healthBar = GameObject.Find("HealthBarSlider").GetComponent<Slider>();
        input = GameObject.Find("Player").GetComponent<vThirdPersonInput>();
        animator = GetComponentInChildren<Animator>();
        rbd = GetComponent<Rigidbody>();
        ResetSpeed();
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

    }

    public void AffectSpeed(float percent, bool buff)
    {
        var speedChange = defaultSpeed * percent;
        playerController.freeRunningSpeed = buff ? (defaultSpeed + speedChange) : (defaultSpeed - speedChange) ;
    }

    public IEnumerator TimedAffectSpeed(float percent, float time, bool buff, HashSet<GameObject> set = null) {
        var speedChange = defaultSpeed * percent;
        playerController.freeRunningSpeed = buff ? (defaultSpeed + speedChange) : (defaultSpeed - speedChange) ;
        yield return new WaitForSeconds(time);
        if (set != null) set.Remove(gameObject);
        ResetSpeed();
    }

    private void OnTriggerExit(Collider other)
    {
        
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PetBehaviour : NetworkBehaviour
{
    public float healAmount;
    public float speedBuffPercent;
    public float speedBuffDuration;

    Animator animator;
    ParticleSystem heart;

    void Start()
    {
        heart = GetComponent<ParticleSystem>();
        heart.Play();
        StartCoroutine(
            EndPet()
        );
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        if (other.gameObject.tag != "Player") return;
        other.GetComponent<DamagablePlayer>().Heal(healAmount);
    }

    private IEnumerator EndPet()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FoodBehaviour : NetworkBehaviour
{
    [Header("Food Settings")]
    public int healAmount;

    void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        if (other.gameObject.tag != "Player") return;
        other.gameObject.GetComponent<DamagablePlayer>().Heal(healAmount);
        Destroy(gameObject);
    }
}

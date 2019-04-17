using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBehaviour : MonoBehaviour
{
    [Header("Food Settings")]
    public int healAmount;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        other.gameObject.GetComponent<DamagablePlayer>().RpcHeal(healAmount);
        Destroy(gameObject);
    }
}

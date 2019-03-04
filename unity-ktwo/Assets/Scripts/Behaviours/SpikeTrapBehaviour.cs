﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapBehaviour : MonoBehaviour
{
    public float damageAmount;
    Collider lastCollided;
    
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Zombie") return;
        lastCollided = other;
        other.gameObject.GetComponent<DamagableEnemy>().Hit(damageAmount, Vector3.zero);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Zombie") return;
        other.gameObject.GetComponent<EnemyController>().ResumeMovement();
    }

    private void OnDestroy()
    {
        if (!lastCollided) return;
        OnTriggerExit(lastCollided);
    }
}
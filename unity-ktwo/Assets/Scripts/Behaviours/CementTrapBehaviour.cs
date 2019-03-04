using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CementTrapBehaviour : MonoBehaviour
{
    public float speedSlowPercent;
    Collider lastCollided;
    
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Zombie") return;
        lastCollided = other;
        other.gameObject.GetComponent<EnemyController>().AffectSpeed(speedSlowPercent, false);
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

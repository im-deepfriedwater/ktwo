using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PavedRoadBehaviour : MonoBehaviour
{
    public float speedBoostPercent;
    Collider lastCollided;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        lastCollided = other;
        other.GetComponent<PlayerBehaviour>().SpeedBoost(speedBoostPercent);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        other.GetComponent<PlayerBehaviour>().ResetSpeed();
    }

    private void OnDestroy()
    {
        if (!lastCollided) return;
        OnTriggerExit(lastCollided);
    }
}

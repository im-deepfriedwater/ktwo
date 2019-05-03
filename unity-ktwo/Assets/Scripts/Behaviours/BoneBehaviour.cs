using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BoneBehaviour : NetworkBehaviour
{
    public float duration;
    public float speedBuffPercent;
    public float speedBuffDuration;

    void Start()
    {
        if (!isServer) return;
        GlobalTaunt();
        StartCoroutine(
            ResetGlobalTarget()
        );
    }

    void GlobalTaunt()
    {
        EnemyManager.instance.SetGlobalTarget(gameObject);
    }

    private IEnumerator ResetGlobalTarget()
    {
        yield return new WaitForSeconds(duration);
        EnemyManager.instance.ResetGlobalTarget();
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        if (other.gameObject.tag != "Player") return;
        other.GetComponent<PlayerBehaviour>().TimedAffectSpeedCR(speedBuffPercent, speedBuffDuration, true);
        Destroy(gameObject.GetComponent<SphereCollider>());
    }
}

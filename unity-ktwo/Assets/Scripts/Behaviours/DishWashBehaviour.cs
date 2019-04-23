using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DishWashBehaviour : NetworkBehaviour
{
    [Header("Dish Wash Settings")]
    public float speedDebuffPercent;
    public float duration;
    public float abilityDuration;

    private GameObject player;
    private Vector3 offsetFromPlayer;
    private bool active = true;
    private HashSet<GameObject> affectedEntities = new HashSet<GameObject>();
    private ParticleSystem bubbles;

    void Start()
    {
        player = (!isServer) ? PlayerManager.instance.players[0] : PlayerManager.instance.player;
        transform.parent = player.transform;
        bubbles = gameObject.GetComponent<ParticleSystem>();
        StartCoroutine(Deactivate(abilityDuration));
    }

    void Update()
    {
        if (!active && affectedEntities.Count == 0) Destroy(gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        if (!isServer) return;
        if (other.gameObject.tag != "Zombie") return;

        other.gameObject.GetComponent<EnemyController>().AffectSpeed(speedDebuffPercent, false);
    }

    void OnTriggerExit(Collider other)
    {
        if (!isServer) return;
        if (other.gameObject.tag != "Zombie") return;
        other.GetComponent<EnemyController>().ResetSpeed();
        affectedEntities.Add(other.gameObject);

        other.GetComponent<EnemyController>().TimedAffectSpeed(speedDebuffPercent, duration, false);
        StartCoroutine(
            RemoveFromHashSet(other.gameObject, duration)
        );
    }

    IEnumerator Deactivate(float time)
    {
        yield return new WaitForSeconds(time);
        active = false;
        transform.localScale = Vector3.zero;
        bubbles.Stop();
    }

    public IEnumerator RemoveFromHashSet(GameObject entity, float time = 0f)
    {
        yield return new WaitForSeconds(time);
        affectedEntities.Remove(entity);
    }
}

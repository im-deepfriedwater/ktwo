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
        Debug.Log("is this running");
        other.GetComponent<EnemyController>().TimedAffectSpeed(speedDebuffPercent, duration, false);
        StartCoroutine(
            RemoveFromHashSet(other.gameObject, duration)
        );
    }

    void OnDestroy()
    {
        Debug.Log("entity count = " + affectedEntities.Count);
        foreach (var entity in affectedEntities)
        {
            entity.GetComponent<EnemyController>().ResetSpeed();
            Debug.Log("zombie " + entity.name);
        }
        Debug.Log("prefab destroyed");
    }

    IEnumerator Deactivate(float time)
    {
        yield return new WaitForSeconds(time);
        bubbles.Stop();
        transform.localScale = Vector3.zero;
        active = false;
        Debug.Log("bubble visual stopped");
    }

    public IEnumerator RemoveFromHashSet(GameObject entity, float time = 0f)
    {
        yield return new WaitForSeconds(time);
        affectedEntities.Remove(entity);
    }
}

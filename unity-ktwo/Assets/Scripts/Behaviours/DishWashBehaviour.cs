using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishWashBehaviour : MonoBehaviour
{
    [Header("Dish Wash Settings")]
    public float speedDebuffPercent;

    private GameObject player;
    private Vector3 offsetFromPlayer;

    void Start()
    {
        player = GameObject.Find("player");
        offsetFromPlayer = player.transform.position - gameObject.transform.position;
    }

    void Update()
    {
        gameObject.transform.position = player.transform.position + offsetFromPlayer;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Zombie") return;

        other.gameObject.GetComponent<EnemyController>().AffectSpeed(speedDebuffPercent, false);
    }
}

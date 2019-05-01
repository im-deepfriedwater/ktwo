using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class EnemyController : NetworkBehaviour
{
    public float attackCooldown;
    public float timeUntilDamageCalculation;
    public float lookRadius = 10f;
    public float damage;
    public float attackAnimationSpeed;
    public float defaultSpeed;

    float previousAnimatorSpeed;

    public bool isAttackOnCooldown = false;
    public bool isAttacking = false;
    public bool hitboxActivated = false;
    public bool hasAttacked = false;

    public GameObject target;

    bool CountDownForAttackHitBoxCoroutineStarted = false;
    bool StartAttackCooldownCoroutineStarted = false;

    public bool turned = false;

    Transform currentTransform;
    NavMeshAgent agent;
    Animator animator;
    Rigidbody rbd;

    // Start is called before the first frame update
    void Start()
    {
        currentTransform = GetComponent<Transform>();
        animator = GetComponentInChildren<Animator>();
        rbd = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(turned && target == null)
        {
            FindNewTarget();
        }
        if (isAttacking)
        {
            agent.isStopped = true;
            rbd.velocity = Vector3.zero;
            // the checks for OnTriggerStay execute sequentially 
            // after the isAttacking. so if it is still false on next
            // frame there must not be anyone to attack.
            // this is a little logic hack that really encourages
            // a rewrite of the enemy controller post-presentation...
            isAttacking = false;
        }
        else
        {
            SetAttackAnimation(false);

            if (target == null)
            {
                FindNewTarget();
                // if target is STILL null there must not
                // be anyone so do nothing.
                if (target == null)
                {
                    return;
                }
            }
            else
            {
                float distance = Vector3.Distance(target.transform.position, transform.position);
                agent.SetDestination(target.transform.position);
                agent.isStopped = false;
                FaceTarget();
            }
        }
    }

    void FindNewTarget()
    {
        if (!isServer) return;
        try
        {
            var chosenTarget = turned ? EnemyManager.instance.GetRandomZombie()
                : PlayerManager.instance.TargetRandomPlayer();
            while (chosenTarget == gameObject)
            {
                chosenTarget = EnemyManager.instance.GetRandomZombie();
            }
            target = chosenTarget;
            RpcSetTarget(target.GetComponent<NetworkIdentity>());
        } catch(NullReferenceException e)
        {
            Debug.Log("Currently Null!");
        }
    }

    void FaceTarget()
    {
        if (target == null) return;

        Vector3 direction = (target.transform.position - transform.position).normalized;
        if (direction == Vector3.zero)
        {
            direction += Vector3.one;
        }
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Structure") AttackStructure(other);
        if (other.gameObject.tag == "Player" && !turned) AttackPlayer(other);
        if (other.gameObject.tag == "Zombie" && turned) AttackZombie(other);
    }

    void OnTriggerExit(Collider other)
    {
        isAttacking = false;
    }

    void AttackStructure(Collider other)
    {
        var structure = other.gameObject.GetComponent<DamagableStructure>();
        if (structure == null) return;

        SetAttackAnimation(true);
        isAttacking = true;
        agent.isStopped = true;

        if (!isServer) return;

        if (isAttacking && !isAttackOnCooldown && !hitboxActivated)
        {
            if (!CountDownForAttackHitBoxCoroutineStarted)
            {
                StartCoroutine(CountDownForAttackHitBox());
            }
        }
        else if (isAttacking && !isAttackOnCooldown && hitboxActivated)
        {
            structure.Hit(damage);
            hitboxActivated = false;
            isAttackOnCooldown = true;
            if (StartAttackCooldownCoroutineStarted)
            {
                return;
            }
            StartCoroutine("StartAttackCooldown");
        }
    }

    void AttackPlayer(Collider other)
    {
        var player = other.gameObject.GetComponent<DamagablePlayer>();
        if (player == null || other.gameObject.GetComponent<PlayerBehaviour>().isDead) return;

        SetAttackAnimation(true);
        isAttacking = true;
        agent.isStopped = true;

        if (!isServer) return;

        if (isAttacking && !isAttackOnCooldown && !hitboxActivated)
        {
            if (!CountDownForAttackHitBoxCoroutineStarted)
            {
                StartCoroutine(CountDownForAttackHitBox());
            }
        }
        else if (isAttacking && !isAttackOnCooldown && hitboxActivated)
        {
            hasAttacked = true;
            Vector3 direction = currentTransform.forward;
            player.Hit(damage, direction, true);
            hitboxActivated = false;
            isAttackOnCooldown = true;
            if (StartAttackCooldownCoroutineStarted)
            {
                return;
            }
            StartCoroutine(StartAttackCooldown());
        }

        if (player.currentHealth <= 0)
        {
            FindNewTarget();
        }
    }

    void AttackZombie(Collider other)
    {
        var zombie = other.gameObject.GetComponent<DamagableEnemy>();
        if (zombie == null)
        {
            FindNewTarget();
        }
        SetAttackAnimation(true);
        isAttacking = true;
        agent.isStopped = true;

        if (!isServer) return;

        if (isAttacking && !isAttackOnCooldown && !hitboxActivated)
        {
            if (!CountDownForAttackHitBoxCoroutineStarted)
            {
                StartCoroutine(CountDownForAttackHitBox());
            }
        }
        else if (isAttacking && !isAttackOnCooldown && hitboxActivated)
        {
            hasAttacked = true;
            Vector3 direction = currentTransform.forward;
            zombie.Hit(damage, direction, true);
            hitboxActivated = false;
            isAttackOnCooldown = true;
            if (StartAttackCooldownCoroutineStarted) return;
            StartCoroutine(StartAttackCooldown());
        }

        if (zombie == null)
        {
            FindNewTarget();
        }

        // if we have attacked, attempt to reset state so the zombie
        // does not stay in place attacking. 
        if (hasAttacked)
        {
            isAttacking = false;
        }
    }

    public void AffectSpeed(float percent, bool buff)
    {
        var speedChange = defaultSpeed * percent;
        agent.speed = buff ? (defaultSpeed + speedChange) : (defaultSpeed - speedChange);
    }

    public void TimedAffectSpeed(float percent, float time, bool buff)
    {
        StartCoroutine(TimedAffectSpeedCR(percent, time, buff));
    }

    [ClientRpc]
    public void RpcTimedAffectSpeed(float percent, float time, bool buff)
    {
        if(!isServer)
        {
            TimedAffectSpeed(percent, time, buff);
        }
    }

    public IEnumerator TimedAffectSpeedCR(float percent, float time, bool buff)
    {
        var speedChange = defaultSpeed * percent;
        agent.speed = buff ? (defaultSpeed + speedChange) : (defaultSpeed - speedChange);
        yield return new WaitForSeconds(time);
        ResetSpeed();
    }

    public void ResetSpeed()
    {
        agent.speed = defaultSpeed;
    }

    public void TurnAgainstOwn(float time)
    {
        StartCoroutine(TurnAgainstOwnCR(time));
    }

    public IEnumerator TurnAgainstOwnCR(float time)
    {
        var zombie = gameObject;
        turned = true;
        FindNewTarget();
        yield return new WaitForSeconds(time);
        turned = false;
        FindNewTarget();
        //StopAllCoroutines();
        //SetAttackAnimation(false);
        //isAttackOnCooldown = false;
        //isAttacking = false;
        //hitboxActivated = false;
        //CountDownForAttackHitBoxCoroutineStarted = false;
        //StartAttackCooldownCoroutineStarted = false;

        if (isServer)
        {
            agent.SetDestination(target.transform.position);
        }
    }

    IEnumerator StartAttackCooldown()
    {
        StartAttackCooldownCoroutineStarted = true;
        yield return new WaitForSeconds(attackCooldown);
        isAttackOnCooldown = false;
        StartAttackCooldownCoroutineStarted = false;
    }

    IEnumerator CountDownForAttackHitBox()
    {
        CountDownForAttackHitBoxCoroutineStarted = true;
        yield return new WaitForSeconds(timeUntilDamageCalculation);
        hitboxActivated = true;
        CountDownForAttackHitBoxCoroutineStarted = false;
    }

    void SetAttackAnimation(bool value)
    {
        animator.SetBool("IsAttacking", value);
    }

    IEnumerator CalculateAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttackOnCooldown = true;
    }

    [ClientRpc]
    void RpcSetTarget(NetworkIdentity target)
    {
        if (target == null)
        {
            Debug.LogWarning("Tried setting enemy target to an entity that does not exist on the client!");
            return;
        }

        this.target = target.gameObject;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
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
    public bool hasAttacked = false; // False on spawn, should be set true after first attack.

    bool CountDownForAttackHitBoxCoroutineStarted = false;
    bool StartAttackCooldownCoroutineStarted = false;

    Transform currentTransform;
    Transform target;
    NavMeshAgent agent;
    Animator animator;
    Rigidbody rbd;

    // Start is called before the first frame update
    void Start()
    {   
        currentTransform = GetComponent<Transform>();
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        rbd = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (!isAttacking && distance <= lookRadius)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            if (distance <= agent.stoppingDistance)
            {
                FaceTarget();
            }
        }

        if (isAttacking)
        {
            agent.isStopped = true;
            rbd.velocity = Vector3.zero;
        }
    }

    void FaceTarget ()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Structure") AttackStructure(other);

        if (other.gameObject.tag == "Player") AttackPlayer(other);

        if (other.gameObject.tag != "Structure" && other.gameObject.tag != "Player") return;
    }

    void OnTriggerExit(Collider other)
    {
        ResumeMovement();
    }

    void AttackStructure(Collider other)
    {

        var structure = other.gameObject.GetComponent<DamagableStructure>();
        if (structure == null) return;

        SetAttackAnimation(true);
        isAttacking = true;
        agent.isStopped = true;

        if (isAttacking && !isAttackOnCooldown && !hitboxActivated)
        {
            if (!CountDownForAttackHitBoxCoroutineStarted)
            {
                StartCoroutine("CountDownForAttackHitBox");
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

        if (structure.currentHealth <= 0) 
        {
            ResumeMovement();
            return;
        }
    }

    void AttackPlayer(Collider other)
    {
        var player = other.gameObject.GetComponent<DamagablePlayer>();
        if (player == null) return;

        SetAttackAnimation(true);
        isAttacking = true;
        agent.isStopped = true;

        if (isAttacking && !isAttackOnCooldown && !hitboxActivated)
        {
            if (!CountDownForAttackHitBoxCoroutineStarted)
            {
                StartCoroutine("CountDownForAttackHitBox");
            }
        }
        else if (isAttacking && !isAttackOnCooldown && hitboxActivated)
        {
            hasAttacked = true;
            Vector3 direction = currentTransform.forward;
            player.Hit(damage, direction);
            hitboxActivated = false;
            isAttackOnCooldown = true;
            if (StartAttackCooldownCoroutineStarted)
            {
                return;
            }
            StartCoroutine("StartAttackCooldown");
        }
    }

    public void AffectSpeed(float percent, bool buff)
    {
        var speedChange = defaultSpeed * percent;
        agent.speed = buff ? (defaultSpeed + speedChange) : (defaultSpeed - speedChange) ;
    }

    public IEnumerator TimedAffectSpeed(float percent, float time, bool buff, HashSet<GameObject> set = null) {
        var zombie = gameObject;
        var speedChange = defaultSpeed * percent;
        agent.speed = buff ? (defaultSpeed + speedChange) : (defaultSpeed - speedChange) ;
        yield return new WaitForSeconds(time);
        if (set != null) set.Remove(zombie);
        ResetSpeed();
    }

    public void ResetSpeed()
    {
        agent.speed = defaultSpeed;
    }

    public void ResumeMovement ()
    {
        SetAttackAnimation(false);
        isAttackOnCooldown = false;
        isAttacking = false;
        hitboxActivated = false;
        CountDownForAttackHitBoxCoroutineStarted = false;
        StartAttackCooldownCoroutineStarted = false;
        agent.SetDestination(target.position);
        StopAllCoroutines();
    }

    IEnumerator StartAttackCooldown ()
    {
        StartAttackCooldownCoroutineStarted = true;
        yield return new WaitForSeconds(attackCooldown);
        isAttackOnCooldown = false;
        StartAttackCooldownCoroutineStarted = false;
    }

    IEnumerator CountDownForAttackHitBox () 
    {
        CountDownForAttackHitBoxCoroutineStarted = true;
        yield return new WaitForSeconds(timeUntilDamageCalculation);
        hitboxActivated = true;
        CountDownForAttackHitBoxCoroutineStarted = false;
    }

    void SetAttackAnimation (bool value)
    {
        animator.SetBool("IsAttacking", value);
    }

    IEnumerator CalculateAttackCooldown ()
    {   
        yield return new WaitForSeconds(attackCooldown);
        isAttackOnCooldown = true;
    }
}

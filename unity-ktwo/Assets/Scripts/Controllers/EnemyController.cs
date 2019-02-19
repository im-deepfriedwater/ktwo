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

    float previousAnimatorSpeed;

    public bool isAttackOnCooldown = false;
    public bool isAttacking = false;
    public bool hitboxActivated = false;
    public bool hasAttacked = false; // False on spawn, should be set true after first attack.

    bool CountDownForAttackHitBoxCourtineStarted = false;
    bool StartAttackCooldownCourtineStarted = false;
    bool WaitForAttackAnimationToFinishCourtineStarted = false;

    Transform currentTransform;
    Transform target;
    NavMeshAgent agent;
    Animator animator;
    GameObject previouslyCollided;

    // Start is called before the first frame update
    void Start()
    {   
        currentTransform = GetComponent<Transform>();
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (previouslyCollided == null && hasAttacked)
        {
            Debug.Log("Itsa me Previously Collided Garbgio");
            ResumeMovement();
        }

        float distance = Vector3.Distance(target.position, transform.position);

        if (!isAttacking && distance <= lookRadius)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            if (distance <= agent.stoppingDistance)
            {
                FaceTarget();
            }
        } else 
        {
            agent.isStopped = true;
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

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Destructable" || other.gameObject.tag == "Player")
        {   
            SetAttackAnimation(true);
            previouslyCollided = other.gameObject;
            isAttacking = true;
            agent.isStopped = true;
        }
    }

    void OnCollisionStay(Collision other)
    {
        Debug.Log("colliding...");
        if (isAttacking && !isAttackOnCooldown && !hitboxActivated)
        {
            if (!CountDownForAttackHitBoxCourtineStarted)
            {
                StartCoroutine("CountDownForAttackHitBox");
            }
        }
        else if (isAttacking && !isAttackOnCooldown && hitboxActivated && other.gameObject.tag == "Destructable")
        {
            other.gameObject.GetComponent<Damagable>().Hit(damage);
            hitboxActivated = false;
            isAttackOnCooldown = true;
            if (StartAttackCooldownCourtineStarted)
            {
                return;
            }
            StartCoroutine("StartAttackCooldown");
        } else if (isAttacking && !isAttackOnCooldown && hitboxActivated && other.gameObject.tag == "Player")
        {
            hasAttacked = true;
            Debug.Log("tryn");
            Vector3 direction = currentTransform.forward;
            other.gameObject.GetComponent<DamagablePlayer>().Hit(damage, direction);
            hitboxActivated = false;
            isAttackOnCooldown = true;
            if (StartAttackCooldownCourtineStarted)
            {
                return;
            }
            StartCoroutine("StartAttackCooldown");
        }
    }

    void OnCollisionExit(Collision other)
    {
        Debug.Log("yuh");
        if (WaitForAttackAnimationToFinishCourtineStarted)
        {
            return;
        } 
        StartCoroutine("WaitForAttackAnimationToFinish");
    }

    void ResumeMovement ()
    {
        Debug.Log("RESET");
        SetAttackAnimation(false);
        isAttackOnCooldown = false;
        isAttacking = false;
        hitboxActivated = false;
        WaitForAttackAnimationToFinishCourtineStarted = false;
        CountDownForAttackHitBoxCourtineStarted = false;
        StartAttackCooldownCourtineStarted = false;
        agent.SetDestination(target.position);
        StopAllCoroutines();
    }

    IEnumerator WaitForAttackAnimationToFinish ()
    {
        WaitForAttackAnimationToFinishCourtineStarted = true;
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            yield return null;
        }
        Debug.Log("what the heck is triggering me");
        WaitForAttackAnimationToFinishCourtineStarted = false;
    }

    IEnumerator StartAttackCooldown ()
    {
        StartAttackCooldownCourtineStarted = true;
        yield return new WaitForSeconds(attackCooldown);
        isAttackOnCooldown = false;
        StartAttackCooldownCourtineStarted = false;
    }

    IEnumerator CountDownForAttackHitBox () 
    {
        CountDownForAttackHitBoxCourtineStarted = true;
        yield return new WaitForSeconds(timeUntilDamageCalculation);
        hitboxActivated = true;
        CountDownForAttackHitBoxCourtineStarted = false;
    }

    void SetAttackAnimation (bool value)
    {
        animator.SetBool("IsAttacking", value);
    }

    IEnumerator CalculateAttackCooldown ()
    {   
        float currentTime = 0;
        while (currentTime <= attackCooldown)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        isAttackOnCooldown = true;
    }
}

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

    bool CountDownForAttackHitBoxCoroutineStarted = false;
    bool StartAttackCooldownCoroutineStarted = false;
    
    private bool turned = false;

    Transform currentTransform;
    GameObject target;
    NavMeshAgent agent;
    Animator animator;
    Rigidbody rbd;

    // Start is called before the first frame update
    void Start()
    {   
        currentTransform = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        rbd = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) FindNewTarget();

        float distance = Vector3.Distance(target.transform.position, transform.position);

        if (!isAttacking && distance <= lookRadius)
        {
            agent.isStopped = false;
            agent.SetDestination(target.transform.position);
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

    void FindNewTarget()
    {
        // this might all break when we have more than one player
        if (turned)
        {
            var zombies = GameObject.FindGameObjectsWithTag("Zombie");
            target = zombies[Random.Range(0, zombies.Length)];
        } else 
        {
            target = PlayerManager.instance.GetClosestPlayer(gameObject.transform.position);
        }
    }

    void FaceTarget ()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        if (direction == Vector3.zero)
        {
            direction += Vector3.one;
        }
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

        if (other.gameObject.tag == "Player" && !turned) AttackPlayer(other);

        if (other.gameObject.tag == "Zombie" && turned) AttackZombie(other);
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

        if (player.currentHealth <= 0) 
        {
            // FindNewTarget(); may need this in the future
            ResumeMovement();
            return;
        }
    }

    void AttackZombie(Collider other)
    {
        var zombie = other.gameObject.GetComponent<DamagableEnemy>();
        if (zombie == null) return;

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
            zombie.Hit(damage, direction);
            hitboxActivated = false;
            isAttackOnCooldown = true;
            if (StartAttackCooldownCoroutineStarted)
            {
                return;
            }
            StartCoroutine("StartAttackCooldown");
        }

        if (zombie.currentHealth <= 0) 
        {
            FindNewTarget();
            ResumeMovement();
            return;
        }
    }

    public void AffectSpeed(float percent, bool buff)
    {
        var speedChange = defaultSpeed * percent;
        agent.speed = buff ? (defaultSpeed + speedChange) : (defaultSpeed - speedChange) ;
    }

    public IEnumerator TimedAffectSpeed(float percent, float time, bool buff, HashSet<GameObject> set = null) 
    {
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

    public IEnumerator TurnAgainstOwn(float time, HashSet<GameObject> set = null)
    {
        var zombie = gameObject;
        var oldTarget = target;
        var activeZombies = GameObject.FindGameObjectsWithTag("Zombie");
        target = activeZombies[Random.Range(0, activeZombies.Length)];
        turned = true;
        yield return new WaitForSeconds(time);
        turned = false;
        target = oldTarget;
        if (set != null) set.Remove(zombie);
    }

    public void ResumeMovement ()
    {
        SetAttackAnimation(false);
        isAttackOnCooldown = false;
        isAttacking = false;
        hitboxActivated = false;
        CountDownForAttackHitBoxCoroutineStarted = false;
        StartAttackCooldownCoroutineStarted = false;
        agent.SetDestination(target.transform.position);
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

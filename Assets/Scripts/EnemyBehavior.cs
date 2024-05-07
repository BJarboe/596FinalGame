using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour, IHear
{
    private Animator anim;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;

    private Vector3 soundWalkpoint;

    //Sound Response
    private bool isRespondingToSound = false;
    //Patrolling
    [SerializeField] private Vector3 walkpoint;
    [SerializeField] private bool walkPointSet;
    [SerializeField] private float walkPointRange;
    [SerializeField] private float slowSpeed = 3f;
    [SerializeField] private float fastSpeed = 5f;

    //Attacking
    [SerializeField] private float rotationSpeed = 7f;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private bool alreadyAttacked;
    [SerializeField] private bool isAttacking;

    //States
    [SerializeField] private float sightRange, attackRange;
    [SerializeField] private bool playerInSightRange, playerInAttackRange;

    
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        //spawner = GameObject.Find("Spawner").GetComponent<EnemySpawner>();
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        CheckSoundResponse();
        SetIsAttacking(false);

        if (isRespondingToSound == false)
        {
            if (!playerInSightRange && !playerInAttackRange)
            {
                Patrolling();
            }
            if (playerInSightRange && !playerInAttackRange)
            {
                ChasePlayer();
            }
            if (playerInSightRange && playerInAttackRange)
            {
                AttackPlayer();
            }
        }

    }

    public void Patrolling()
    {
        anim.SetInteger("State", 0);
        agent.speed = slowSpeed;

        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkpoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkpoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkpoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    public void ChasePlayer()
    {
        anim.SetInteger("State", 1);
        agent.speed = fastSpeed;
        agent.SetDestination(player.position);
    }

    public void AttackPlayer()
    {
        anim.SetInteger("State", 2);
        SetIsAttacking(true);
        agent.speed = 0f;
        agent.SetDestination(transform.position);

        //anim.SetInteger("AttackIndex", Random.Range(0, 5));

        //agent.speed = 0f;

        //agent.SetDestination(player.position);

        Vector3 dir = player.position - transform.position;
        dir.y = 0.0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);


        if (!alreadyAttacked)
        {
            //agent.SetDestination(transform.position);
            anim.SetInteger("AttackIndex", Random.Range(0, 5));

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }

    public void SetIsAttacking(bool isAttacking)
    {
        this.isAttacking = isAttacking;
    }

    public bool GetIsAttacking()
    {
        return isAttacking;
    }

    public void RespondToSound(Sound sound)
    {
        Debug.Log("Respond to sound called");
        //set animation to surprised

        //save walkpoint
        soundWalkpoint = sound.pos;

        //go to item
        agent.SetDestination(sound.pos);
        agent.isStopped = false;
    }

    public void SetIsRespondingToSound(bool isRespondingToSound)
    {
        this.isRespondingToSound = isRespondingToSound;
    }

    public void CheckSoundResponse()
    {
        Vector3 distanceToSoundWalkPoint = transform.position - soundWalkpoint;

        if (distanceToSoundWalkPoint.magnitude < 1f)
        {
            isRespondingToSound = false;
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}

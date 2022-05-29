using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * enemy ai behaviour
 */

public class EnemyAI : MonoBehaviour
{

    public enum EnemyState
    {

        Idle,
        Attack,
        Dancing

    }
    
    public float coolDownTime = 8.0f; // attack cooldown
    public float attackPoint = 10f;
    public Vector3 offset = new Vector3(0f, 0f, 2.5f);

    private EnemyState state;

    private Transform targetPlayer;
    private NavMeshAgent agent;
    private Animator anim;
    private AudioSource[] sounds;

    private bool isCoolDown;
    private bool isAttack;
    private bool isWalking;

    void Awake()
    {

        targetPlayer = null;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        sounds = GetComponents<AudioSource>();

        isCoolDown = true;
        isAttack = false;
        isWalking = false;

        // cool down for player to ready when first enter into the game
        StartCoroutine(CoolDown());

    }

    void Update()
    {

        if (!isCoolDown)
        {
            if (targetPlayer != null)
                ChasePlayer();
            else
                FindPlayer();
            
        }

        anim.SetBool("isWalking", isWalking);

    }

    // chase the player
    void ChasePlayer()
    {

        if (agent.remainingDistance < agent.stoppingDistance && !isCoolDown)
        {
            // set destination to player position
            NavMeshHit navHit;
            NavMesh.SamplePosition(targetPlayer.position + offset, out navHit, 10f, NavMesh.AllAreas);
            agent.SetDestination(navHit.position);

        }

        transform.LookAt(targetPlayer);

        float distance = Vector3.Distance(targetPlayer.position, transform.position);

        // distance between player and ai is less than 2.5f, then attack player
        if (distance < 0.5f)
            StartCoroutine(AttackPlayer());

        isWalking = distance < 0.05 ? false : true;

    }

    void FindPlayer()
    {

        if (agent.remainingDistance < agent.stoppingDistance)
        {
            // set the next destination
            Vector3 randomSpot = Random.insideUnitSphere * 10f;
            NavMeshHit navHit;
            NavMesh.SamplePosition(transform.position + randomSpot, out navHit, 10f, NavMesh.AllAreas);
            agent.SetDestination(navHit.position);

        }

        isWalking = true;

    }

    // attack player 
    IEnumerator AttackPlayer()
    {

        state = EnemyState.Attack;
        isCoolDown = true;
        isAttack = true;
        anim.SetTrigger("triggerAttack");
        sounds[1].Play();
        yield return new WaitForSeconds(1f);
        StartCoroutine(CoolDown());
        yield return null;

    }

    // cool down the attack
    IEnumerator CoolDown()
    {

        yield return new WaitForSeconds(0.5f);
        state = EnemyState.Dancing;
        isAttack = false;
        sounds[0].Play();
        yield return new WaitForSeconds(coolDownTime);
        state = EnemyState.Idle;
        isCoolDown = false;
        anim.SetTrigger("stopDancing");
        yield return null;

    }

    // get and set function

    public EnemyState GetState()
    {

        return state;

    }

    public void SetTargetPlayer(Transform player)
    {

        targetPlayer = player;

    }

    public bool GetIsAttack()
    {

        return isAttack;

    }

    public bool GetIsCoolDown()
    {

        return isCoolDown;

    }

    public float GetCoolDownTime()
    {

        return coolDownTime;

    }

    public float GetAttackPoint() {

        return attackPoint;
    
    }

}

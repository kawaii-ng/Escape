using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    public EnemyAI enemy;
    private PlayerController player;
    private bool isAttacking;
    private bool isValidAttack;
    private float attackPoint;


    void Start()
    {

        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        isAttacking = false;
        isValidAttack = false;
        attackPoint = enemy.GetAttackPoint();

    }

    void Update()
    {

        //Debug.Log("EnemeyAttack: " + isAttacking);

        if (enemy.GetState() == EnemyAI.EnemyState.Attack)
            if (!isAttacking)
                StartCoroutine(Attack());

    }


    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            isValidAttack = true;

        }

    }

    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            isValidAttack = false;

        }

    }

    IEnumerator Attack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(1f);
        if (isValidAttack)
            player.ChangeHealth(attackPoint);
        yield return new WaitForSeconds(enemy.GetCoolDownTime() - 1f);
        isAttacking = false;
        yield return null;

    }
}

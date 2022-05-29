using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * check whether enemy can see the player 
 */

public class EnemySight : MonoBehaviour
{
    public EnemyAI enemy;

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
            enemy.SetTargetPlayer(other.gameObject.transform);

    }


    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player")
            enemy.SetTargetPlayer(null);

    }
}

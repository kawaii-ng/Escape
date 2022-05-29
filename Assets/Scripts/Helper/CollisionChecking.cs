using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecking : MonoBehaviour
{

    public enum CheckType { 
    
        Player,
        Enemy
    
    }

    public CheckType type = CheckType.Player;

    private bool isEnter = false;
    private PlayerController player;

    void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag == "Player" && type == CheckType.Player)
        {
            player = other.GetComponent<PlayerController>();
            isEnter = true;
        }

        if (other.gameObject.tag == "Enemy" && type == CheckType.Enemy)
        {
            isEnter = true;
        }


    }

    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player" && type == CheckType.Player)
            isEnter = false;

        if (other.gameObject.tag == "Enemy" && type == CheckType.Enemy)
            isEnter = false;

    }

    public bool GetIsEnter()
    {

        return isEnter;

    }

    public PlayerController GetPlayer()
    {

        return player;

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * delete the overlaping object between two different rooms
 */

public class CheckOverlap : MonoBehaviour
{

    void Awake() {

        // get all the collided objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f);

        foreach(Collider collider in colliders)
        {

            // if collides with wall, destroy it
            if (collider.tag == "Wall")
            {
                Destroy(gameObject);
                return;
            }

        }

        GetComponent<Collider>().enabled = true;
    
    }

}

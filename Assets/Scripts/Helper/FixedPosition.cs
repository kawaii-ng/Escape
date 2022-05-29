using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedPosition : MonoBehaviour
{
    public Transform fixedPoint;

    private GameObject player;

    void Start() {

        player = GameObject.FindGameObjectWithTag("Player");

    }

    public void SetPosition() {

        player.transform.position = new Vector3(fixedPoint.position.x, player.transform.position.y, fixedPoint.position.z);
        player.transform.rotation = fixedPoint.rotation;
    
    }

}

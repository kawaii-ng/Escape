using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{

    public FixedPosition fixedPos;
    public CollisionChecking collisionChecking;
    
    protected PlayerController player;
    protected bool isEnter;
    protected bool isInteracting;

    public virtual void Awake() {

        player = null;
        isEnter = false;
        isInteracting = false;
    
    }

    public virtual void Update()
    {
        player = collisionChecking.GetPlayer();
        isEnter = collisionChecking.GetIsEnter();

        if (isEnter) {

            if (player.GetState() == PlayerController.PlayerState.Using)
                StartAction();
            

            if (isInteracting)
                Interact();

        }
        else
            StopAction();
    }

    public virtual void StartAction() {
        return;
    }

    public virtual void StopAction() {
        return;
    }

    public virtual void Interact() {
        return;
    }


}

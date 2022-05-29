using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{

    public enum DoorState
    {
        Open,
        Close,
        Destroyed
    }

    private Animator doorAnim;
    private AudioSource[] sounds;
    private DoorState currentState;

    private bool isCoolDown;
    private bool isOpen = true;
    private int doorInteractCount;

    /**
     * Initialize the variables
     */

    public override void Awake()
    {

        base.Awake();

        doorAnim = GetComponent<Animator>();
        sounds = GetComponents<AudioSource>();
        currentState = DoorState.Open;

        isCoolDown = false;
        doorInteractCount = 0;

    }

    public override void StartAction() {

        if (!isCoolDown)
        {

            isCoolDown = true;
            isOpen = !isOpen;
            doorAnim.SetBool("isOpen", isOpen);
            player.PlayAnimation("Open");

            if (currentState == DoorState.Close)
                StartCoroutine(OpenDoor());

            if (currentState == DoorState.Open)
                StartCoroutine(CloseDoor());

        }

    }

    /**
     * open the door by updating the rotation
     */

    public IEnumerator OpenDoor()
    {

        if (!isCoolDown) { 
            isCoolDown = true;
            isOpen = !isOpen;
            doorAnim.SetBool("isOpen", isOpen);
        }

        currentState = DoorState.Open;
        doorInteractCount++;

        if (!(doorInteractCount > 3))
            sounds[0].Play();
        else
            StartCoroutine(DestroyDoor());

        yield return new WaitForSeconds(3f);
        isCoolDown = false;
        yield return null;
    }

    /**
     * close the door by updating the rotation
     */

    IEnumerator CloseDoor()
    {

        currentState = DoorState.Close;
        sounds[0].Play();

        yield return new WaitForSeconds(3f);
        isCoolDown = false;
        yield return null;

    }

    /**
     * cool down the interaction
     */

    IEnumerator CoolDownOpen()
    {

        yield return new WaitForSeconds(3f);
        isCoolDown = false;
        yield return null;

    }

    /**
     * deestroy the door 
     */

    IEnumerator DestroyDoor()
    {


        currentState = DoorState.Destroyed;
        sounds[1].Play();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        yield return null;

    }

    /**
     * Get and set state of the door object
     */


    public void SetState(DoorState state)
    {
        currentState = state;
    }


    public DoorState GetState()
    {
        return currentState;
    }

}

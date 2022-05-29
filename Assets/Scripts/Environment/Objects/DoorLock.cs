using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : InteractableObject
{

    public ExitDoor exit;

    private ExitDoor exitGate;
    private AudioSource sound;

    private const float MaxUnloakProgress = 100f;
    private float unlockProgress;
    private float unlockSpeed;

    public override void Awake()
    {
        base.Awake();

        exitGate = gameObject.transform.parent.gameObject.GetComponent<ExitDoor>();
        sound = GetComponent<AudioSource>();

        unlockProgress = 0f;
        unlockSpeed = 20f;

    }

    public override void StartAction() {

        if (unlockProgress < MaxUnloakProgress && GameController.isAlarmed) {

            sound.Play();
            isInteracting = true;
            player.PlayAnimation("EnterPW");
            fixedPos.SetPosition();

            Debug.Log("Door Lock (StartAction): " + player.GetState());

        }
    
    }

    public override void StopAction() {

        StopPressing();
        player = null;

    }

    public override void Interact() {

        if (unlockProgress < MaxUnloakProgress)
        {
            unlockProgress += unlockSpeed * Time.deltaTime;
            player.ShowProgressDisplay(unlockProgress, MaxUnloakProgress);
            Debug.Log("Door Lock (Interact): " + player.GetState());
        }
        else
        {
            StopPressing();
            exit.OpenGate();
        }

    }

    void StopPressing()
    {

        if (player != null)
        {
            sound.Stop();
            if (isInteracting)
            {
                player.PlayAnimation("Stop");
                player.HideProgressDisplay();
            }
            isInteracting = false;

            Debug.Log("Decode Machine (StopAction): ");
        }

    }

}

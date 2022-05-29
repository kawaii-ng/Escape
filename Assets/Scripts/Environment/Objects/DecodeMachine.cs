using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * defining the decode mahcine behaviour
 */

public class DecodeMachine : InteractableObject
{

    private Animator machineAnim;
    private AudioSource[] sounds;

    private const float MaxDecodeProgress = 100f;

    private float decodeProgress;
    private float decodeSpeed;

    public static int decodedCount;
    private int targetNum;

    private bool isShutDown;
    public static bool isCompletedDecode;
      
    public override void Awake()
    {
        base.Awake();
        machineAnim = GetComponent<Animator>();
        sounds = GetComponents<AudioSource>();

        decodeProgress = 0f;
        decodeSpeed = 0f;
        decodedCount = 0;
        targetNum = GameObject.FindWithTag("MapGenerator").GetComponent<MapGenerator>().numOfMachine;

        isShutDown = false;
        isCompletedDecode = false;
    }

    public override void Update()
    {

        base.Update();

        if (isCompletedDecode && !isShutDown)
        {

            /**
             * Since it is enough to escape, all the machines should be shutted down.
             */

            decodeProgress = MaxDecodeProgress;
            machineAnim.SetTrigger("triggerLight");
            isShutDown = true;

        }

    }

    public override void StartAction() { 
    
        if(decodeProgress < MaxDecodeProgress && !isInteracting && !isShutDown)
        {

            isInteracting = true;
            fixedPos.SetPosition();
            decodeSpeed += player.GetDecodeSpeed();
            player.SetState(PlayerController.PlayerState.Using);
            player.ShowProgressDisplay(decodeProgress, MaxDecodeProgress);
            player.PlayAnimation("Decode");
            sounds[0].Play();
            Debug.Log("Decode Machine (StartAction): " + player.GetState());
        }
    
    }

    public override void StopAction() {

        if (!isShutDown) {

            if (isInteracting)
            {
                player.PlayAnimation("Stop");
                player.HideProgressDisplay();
                player.SetState(PlayerController.PlayerState.Idle);

            }
            isInteracting = false;
            decodeSpeed = 0f;
            sounds[0].Stop();

            if (!(decodeProgress < MaxDecodeProgress))
            {
                ++decodedCount;
                sounds[1].Play();
                machineAnim.SetTrigger("triggerLight");
                isShutDown = true;

                if (decodedCount == targetNum)
                    isCompletedDecode = true;

            }

            Debug.Log("Decode Machine (StopAction): ");

        }
    }

    public override void Interact() {

        if (decodeProgress < MaxDecodeProgress)
        {
            player.SetState(PlayerController.PlayerState.Using);
            decodeProgress += decodeSpeed * Time.deltaTime;
            player.ShowProgressDisplay(decodeProgress, MaxDecodeProgress);
            Debug.Log("Decode Machine (Interact): " + player.GetState());
        }
        else {

            player.SetState(PlayerController.PlayerState.Idle);
            StopAction();

        }


    }

    public void SetTargetNum(int num)
    {
        targetNum = num;
    }

    public int GetTargetNum()
    {
        return targetNum;
    }

}

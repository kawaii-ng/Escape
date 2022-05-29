using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidBox : InteractableObject
{

    public float speed = 20f;

    private const float MaxProgress = 100f;
    private float progress;

    public override void Awake()
    {
        base.Awake();
        progress = 0f;
    }

    public override void StartAction() {

        if (!player.IsFullHealth()) {

            isInteracting = true;
            fixedPos.SetPosition();
            player.SetState(PlayerController.PlayerState.Using);
            player.ShowProgressDisplay(progress, MaxProgress);
            Debug.Log("1st Aid Box (StartAction): " + player.GetState());

        }
    
    }

    public override void StopAction() {

        if (player != null)
        {

            if (isInteracting)
            {
                player.HideProgressDisplay();
                player.PlayAnimation("Stop");
                player.SetState(PlayerController.PlayerState.Idle);
            }
            isInteracting = false;
            // reset the progess if not finished
            if (progress < MaxProgress)
                progress = 0f;

            Debug.Log("1st Aid Box (StopAction): ");

        }


    }

    public override void Interact()
    {

        if (!player.IsFullHealth())
        {

            Debug.Log("1st Aid Box (Interact): " + player.GetState());

            if (progress < MaxProgress)
            {
                progress += speed * Time.deltaTime;
                player.SetState(PlayerController.PlayerState.Using);
                player.ShowProgressDisplay(progress, MaxProgress);
                player.PlayAnimation("Heal");
            }
            else
            {
                player.ChangeHealth(10f, false);
                player.HideProgressDisplay();
                player.PlayAnimation("Stop");
                player.SetState(PlayerController.PlayerState.Idle);
                isInteracting = false;
                Destroy(gameObject);
            }


        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/**
 * control the switching between 1st person camera and 3rd person camera
 */

public class CamSwitchController : MonoBehaviour
{

    public CinemachineFreeLook thirdPersonCam;
    public CinemachineVirtualCamera firstPersonCam;

    public static bool isFirstPersonCam = true;

    private MyPlayerInput playerInput;
    private PlayerController player;
    private float showTime;
    private bool isGameplay;
    private MyData mData;

    void Awake() {

        playerInput = new MyPlayerInput();
        mData = SavingSystem.LoadData();
        if (mData != null)
            isFirstPersonCam = mData.GetIsFirstPerson();
        else
            mData = new MyData();

        isGameplay = thirdPersonCam != null && firstPersonCam != null;

        if(isGameplay)
            player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        if (isFirstPersonCam)
            SetFirstPersonCam();
        else
            SetThirdPersonCam();

    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
    
    // for testing
    //void Update() {

    //    if (playerInput.PlayerMain.Skill1.triggered) {

    //        SwitchCam();
        
    //    }
    
    //}

    /**
     * switching camera
     */

    public void SwitchCam() {

        if (isFirstPersonCam)
            SetThirdPersonCam();
        else
            SetFirstPersonCam();

        if(isGameplay)
            Invoke("ShowPlayerHead", showTime);

        
        
    }

    void ShowPlayerHead() {

        player.ShowHead();
    
    }

    /**
     * set to 1st person camera
     */

    private void SetFirstPersonCam() {

        if(isGameplay)
        {
            firstPersonCam.Priority = 10;
            thirdPersonCam.Priority = 0;
            showTime = 3f;
        }
        isFirstPersonCam = true;

        // save data
        mData.SetIsFirstPerson(isFirstPersonCam);
        SavingSystem.SaveData(mData);

    }

    /**
     * set to 3rd person camera
     */
    
    private void SetThirdPersonCam() {

        if (isGameplay) { 
            
            firstPersonCam.Priority = 0;
            thirdPersonCam.Priority = 10;
            showTime = 0f;
        
        }
        isFirstPersonCam = false;

        // save data
        mData.SetIsFirstPerson(isFirstPersonCam);
        SavingSystem.SaveData(mData);

    }

}

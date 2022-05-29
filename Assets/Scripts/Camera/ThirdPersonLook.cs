using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/**
 * setup and control the first person camera
 */

public class ThirdPersonLook : MonoBehaviour
{

    public float lookSpeed = 1;

    private MyPlayerInput playerInput;
    private CinemachineFreeLook cinemachine;

    private void Awake()
    {
        playerInput = new MyPlayerInput();
        cinemachine = GetComponent<CinemachineFreeLook>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    void Update()
    {

        if (!CamSwitchController.isFirstPersonCam)
        {
            // rotate the camera
            Vector2 delta = playerInput.PlayerMain.Look.ReadValue<Vector2>();
            cinemachine.m_XAxis.Value += delta.x * 200 * lookSpeed * Time.deltaTime;

        }

        SetUpVCam();

    }

    /**
     * set up the follow target and look at target if player exists
     */

    public void SetUpVCam()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            Transform target = player.transform;
            cinemachine.LookAt = target;
            cinemachine.Follow = target;
        }
    }

}

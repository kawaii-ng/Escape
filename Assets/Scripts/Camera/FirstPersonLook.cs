using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/**
 * setup and control the first person camera
 */

public class FirstPersonLook : MonoBehaviour {

    public float lookSpeed = 1;
    private MyPlayerInput playerInput;
    private CinemachineVirtualCamera vCam;
    private CinemachineComposer composer;

    private void Awake()
    {

        playerInput = new MyPlayerInput();
        vCam = GetComponent<CinemachineVirtualCamera>();
        composer = vCam.AddCinemachineComponent<CinemachineComposer>();
        composer.m_ScreenX = 0.5f;
        composer.m_ScreenY = 0.5f;

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

        if (CamSwitchController.isFirstPersonCam)
        {

            // rotate camera and player
            Vector2 delta = playerInput.PlayerMain.Look.ReadValue<Vector2>();

            if(delta != Vector2.zero)
            {
                composer.m_ScreenY += delta.y * 5f * lookSpeed * Time.deltaTime;
                Transform player = GameObject.FindWithTag("Player").transform;
                player.Rotate(0f, delta.x * 5f * Mathf.Rad2Deg * Time.deltaTime, 0f);
            }

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
            vCam.Follow = target.GetChild(0).gameObject.transform;
            vCam.LookAt = target.GetChild(1).gameObject.transform;
        }
    }

}
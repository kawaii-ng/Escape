using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * defining the exit door behaviour
 */

public class ExitDoor : MonoBehaviour
{
    private GameObject gate;
    private Animator gateAnim;
    private AudioSource sound;
    private Light light;

    void Awake()
    {
        gate = gameObject.transform.GetChild(0).gameObject;
        gateAnim = gate.GetComponent<Animator>();
        sound = gameObject.GetComponent<AudioSource>();
        light = gameObject.transform.GetChild(1).gameObject.GetComponent<Light>();
    }

    void Update()
    {
        if (DecodeMachine.isCompletedDecode)
            light.intensity = 1;

    }

    public void OpenGate()
    {

        gateAnim.SetTrigger("triggerGate");
        sound.Play();

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * setting menu is show in main menu and gameplay for setting the camera mode
 */

public class SettingMenu : MonoBehaviour
{

    public CamSwitchController camSwitcher;
    public GameObject thirdPersonOption;
    public GameObject firstPersonOption;

    void Start() {

        InitializeCamMode();
    
    }

    void InitializeCamMode() {

        if (CamSwitchController.isFirstPersonCam)
        {
            firstPersonOption.SetActive(true);
            thirdPersonOption.SetActive(false);
        }
        else
        {
            firstPersonOption.SetActive(false);
            thirdPersonOption.SetActive(true);
        }

    }

    public void SwitchCamMode() {

        camSwitcher.SwitchCam();
        InitializeCamMode();

    }

    public void ShowSettingMenu() {

        gameObject.SetActive(true);
        Time.timeScale = 0f;
    
    }

    public void Resume() {

        gameObject.SetActive(false);
        Time.timeScale = 1f;

    }

    public void Quit(bool isQuit = true) {

        if (isQuit)
            Application.Quit();
        else
            SceneManager.LoadScene(0);
    
    }

}

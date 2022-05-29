using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
 * the basic game menu function
 */

public class GameMenu : MonoBehaviour
{

    public GameObject loading;
    public MyData mData;

    public virtual void Awake()
    {
        loading.SetActive(false);
    }

    public virtual void GotoScreen(int index) {

        loading.SetActive(true);
        SceneManager.LoadScene(index);

    }

}

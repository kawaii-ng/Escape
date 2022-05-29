using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    public Slider slider;
    public Text progressLabel;

    public bool isShow = false;

    public void SetCurrentProgress(float progress, float maxProgress)
    {

        slider.maxValue = maxProgress;
        int currentProgress = (int)progress;

        slider.value = currentProgress;

        if (progressLabel) {
        
            string zeros = "";

            if (currentProgress.ToString().Length == 1)
                zeros = "00";
            if (currentProgress.ToString().Length == 2)
                zeros = "0";

            progressLabel.text = "Progress: " + zeros + currentProgress.ToString() + "%";
        
        }

    }

    public void ShowProgressBar()
    {

        if (!isShow)
        {
            isShow = true;
            gameObject.SetActive(isShow);
        }

    }

    public void HideProgressBar()
    {

        if (isShow)
        {
            isShow = false;
            gameObject.SetActive(isShow);
        }

    }

}

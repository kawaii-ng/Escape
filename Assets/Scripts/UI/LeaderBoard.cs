using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LootLocker.Requests;

/**
 * - using LootLock package to implement the leaderboard
 * - this script is to submit the score and show the leaderboard 
 * - show after finishing the gameplay
 */

public class LeaderBoard : GameMenu
{

    [Header ("Leaderboard Info")]
    public int ID;
    public Text[] Entries;
    public GameObject loadingLeaderBoard;
    
    private int maxScores = 7; // maximun number of entry 

    [Header ("Game Result Info")]
    public Text scoreDisplay;
    private int score = 0;

    public override void Awake() {

        base.Awake();

        loadingLeaderBoard.SetActive(true);

        mData = SavingSystem.LoadData();

        if (mData != null)
        {

            float[] endGameData = mData.GetEndGameData();
            score = (int)(endGameData[2] + endGameData[2] * (endGameData[0] * 60 + endGameData[1]));
            scoreDisplay.text = score.ToString();

        }

    }

    // init the LootLocker by submitting the current score 
    private void Start() {

        LootLockerSDKManager.StartSession("Player", (response) =>
        {

            if (response.success)
            {
                Debug.Log("Sucess");
                SubmitScore();
            }
            else { 
            
                Debug.Log("Failed");
            }

        });
    
    }

    // submit the score 
    public void SubmitScore() {

        LootLockerSDKManager.SubmitScore(Random.Range(0f, 1000f).ToString(), int.Parse(scoreDisplay.text), ID, (response) =>
        {

            if (response.success)
            {
                Debug.Log("Sucess");
                ShowScores();
            }
            else
                Debug.Log("Failed");

        });
    
    }

    // show the leaderboard
    public void ShowScores()
    {

        LootLockerSDKManager.GetScoreList(ID, maxScores, (response) => {

            if (response.success)
            {

                for (int i = 0; i < response.items.Length; i++)
                    Entries[i].text = (response.items[i].rank + ". " + response.items[i].score);

                if (response.items.Length < maxScores)
                    for (int i = response.items.Length; i < maxScores; i++)
                        Entries[i].text = (i + 1).ToString() + ". none";

                loadingLeaderBoard.SetActive(false);

            }
            else
                Debug.Log("Failed");

        });

    }

}

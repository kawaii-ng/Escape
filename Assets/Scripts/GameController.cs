using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    //public CollisionChecking[] collisionChecking;
    //public SpawnPointsManager spManager;
    public MapGenerator mapGenerator;
    public Camera cam;
    public Text statusDisplay;
    public ProgressBar timeDisplay;
    public float currentMinute = 2;
    public float currentSecond = 30;

    public static bool isEscape;
    public static bool isDead;

    private PlayerController player;
    private GameObject alarm;
    private AudioSource alarmSound;
    private List<CollisionChecking> escapeAreas;

    private float time;
    private float maxTime;
    public static bool isAlarmed;
    private bool isTimeUp;

    private MyData mData;

    // Start is called before the first frame update
    void Start()
    {

        maxTime = currentMinute * 60f + currentSecond;
        time = maxTime;

        isEscape = false;
        isDead = false;
        alarmSound = gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        escapeAreas = mapGenerator.GetEscapeArea();
        isAlarmed = false;
        isTimeUp = false;

        mData = SavingSystem.LoadData();
        if (mData == null)
            mData = new MyData();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || escapeAreas.Count == 0)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            player.SetUpCam(cam);

            escapeAreas = mapGenerator.GetEscapeArea();
        }
        else
        {

            if (!isAlarmed && DecodeMachine.isCompletedDecode)
                Alarm();

            if (isAlarmed)
                statusDisplay.text = "Electrical lock can be unlocked now.";
            else
            {

                int machineLeft = mapGenerator.GetTotalMachineNum() - DecodeMachine.decodedCount;
                statusDisplay.text = machineLeft.ToString() + " decode machine left.";

            }

            if (isEscape || isDead || isTimeUp)
            {

                int minLeft = (int)(time / 60);
                int secLeft = (int)time - minLeft * 60;

                mData.SetEndGameData(minLeft, secLeft, player.GetHealth());
                SavingSystem.SaveData(mData);
                SceneManager.LoadScene("LeaderBoard");

            }
            else
                isEscape = CheckEscape();

            TimeUpdate();
        }


    }

    public void Alarm()
    {

        alarmSound.Play();
        isAlarmed = true;

    }

    void TimeUpdate() {

        if (time <= 0)
            isTimeUp = true;
        else
            time -= 1f * Time.deltaTime;

        timeDisplay.SetCurrentProgress(time, maxTime);

    }

    bool CheckEscape()
    {

        for(int i = 0; i < escapeAreas.Count; i++)
        {

            if (escapeAreas[i].GetIsEnter())
            {
                player = escapeAreas[i].GetPlayer();
                return true;
            }

        }

        return false;

    }

}

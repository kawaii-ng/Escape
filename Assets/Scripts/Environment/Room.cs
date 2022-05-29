using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * For generating a room with different object and structure
 */

public class Room : MonoBehaviour
{

    [Header ("Room Objects List")]
    public GameObject[] walls;
    public GameObject[] doors;
    public GameObject[] exits;
    public GameObject[] floors;
    public GameObject lightBall;
    public List<CollisionChecking> escapeAreas;

    [Header ("Spawn Point List")]
    public Transform[] characterSpawnPoints;
    public Transform[] machineSpawnPoints;
    public Transform[] boxSpawnPoints;
    public bool isExit = false;

    // generate light ball
    private bool isSetting = false;
    private bool isShowLightBall = false;

    private CollisionChecking escape; 

    void Update() {

        // generating light ball with random time
        if (!isSetting)
        {
            isSetting = true;
            int randTime = (int)Random.Range(0f, 60f);
            Invoke("SetLightBall", randTime);
        }

    }

    /**
     * generate a new room
     */

    public void UpdateRoom(bool[] status, bool isUp = true)
    {
        // determining whether it is a door or wall
        for(int i = 0; i < status.Length; i++)
        {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }

        // generate exit
        if (isExit)
        {
            if (isUp)
            {
                // first exit
                doors[0].SetActive(false);
                walls[0].SetActive(true);
                exits[0].SetActive(true);
                floors[0].SetActive(false);
                escape = escapeAreas[0];

            }
            else
            {
                // last exit
                doors[2].SetActive(false);
                walls[2].SetActive(true);
                exits[1].SetActive(true);
                floors[1].SetActive(false);
                escape = escapeAreas[1];
            }
        }
    }

    // spawn character
    public void SpawnCharacter(GameObject myCharacter)
    {

        int index = GetRandomPosition(characterSpawnPoints.Length);

        if (characterSpawnPoints != null)
            Instantiate(myCharacter, characterSpawnPoints[index].position, characterSpawnPoints[index].rotation, transform);

    }

    // spawn machine
    public void SpawnMachine(GameObject machine) {

        int index = GetRandomPosition(machineSpawnPoints.Length);

        if (machineSpawnPoints != null)
            Instantiate(machine, machineSpawnPoints[index].position, machineSpawnPoints[index].rotation, transform);

    }
    
    // spawn first aid box
    public void SpawnBox(GameObject box) {

        int index = GetRandomPosition(boxSpawnPoints.Length);
    
        if(boxSpawnPoints != null)
            Instantiate(box, boxSpawnPoints[index].position, boxSpawnPoints[index].rotation, transform);

    }

    // randomly generate the position of spawning objects
    int GetRandomPosition(float size)
    {
        return (int)(Random.Range(0f, size));
    }

    // show and hide light ball 
    void SetLightBall() {

        isShowLightBall = !isShowLightBall;
        lightBall.SetActive(isShowLightBall);
        isSetting = false;

    }

    public CollisionChecking GetEscapeArea() {
        return escape;
    }
  

}

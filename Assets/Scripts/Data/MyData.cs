using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * store the gameplay data 
 * (time of completing game, 
 *  remaining health point,
 *  character index)
 */

[System.Serializable]
public class MyData 
{

    public float minuteCompleted;
    public float secondCompleted;
    public float remainingHealth;

    public int characterIndex = 0;

    public bool isFirstPerson;

    public MyData()
    {

        minuteCompleted = 0;
        secondCompleted = 0;
        remainingHealth = 0;
        characterIndex = 0;
        isFirstPerson = false;

    }

    public MyData(MyData data)
    {

        this.minuteCompleted = data.minuteCompleted;
        this.secondCompleted = data.secondCompleted;
        this.remainingHealth = data.remainingHealth;
        this.characterIndex = data.characterIndex;
        this.isFirstPerson = data.isFirstPerson;

    }

    public void SetEndGameData(float min, float sec, float health)
    {

        minuteCompleted = min;
        secondCompleted = sec;
        remainingHealth = health;

    }    

    public float[] GetEndGameData()
    {

        return new float[] { minuteCompleted, secondCompleted, remainingHealth };

    }

    public void SetCharacterIndex(int index)
    {

        characterIndex = index;

    }

    public int GetCharacterIndex()
    {

        return characterIndex;

    }

    public void SetIsFirstPerson(bool isFirstPerson) {

        this.isFirstPerson = isFirstPerson;
    
    }

    public bool GetIsFirstPerson() {

        return isFirstPerson;
    
    }

}

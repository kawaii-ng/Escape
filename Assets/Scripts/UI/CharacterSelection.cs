using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * character selection menu for selecting character
 */

public class CharacterSelection : GameMenu
{

    public List<GameObject> characterList = new List<GameObject>();
    private int currentIndex = 0;

    public override void Awake()
    {
        base.Awake();
        mData = SavingSystem.LoadData();
        if (mData == null)
            mData = new MyData();
    }

    public override void GotoScreen(int index) {

        base.GotoScreen(index);
        SavingSystem.SaveData(mData);

    }

    public void NextCharacter()
    {

        if (currentIndex < characterList.Count - 1)
            currentIndex++;


        for (int i = 0; i < characterList.Count; i++)
            characterList[i].SetActive(false);

        characterList[currentIndex].SetActive(true);

        mData.SetCharacterIndex(currentIndex);


    }

    public void PreviousCharacter()
    {

        if (currentIndex > 0)
            currentIndex--;

        for (int i = 0; i < characterList.Count; i++)
            characterList[i].SetActive(false);

        characterList[currentIndex].SetActive(true);

        mData.SetCharacterIndex(currentIndex);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionMenu : MonoBehaviour
{

    public GameObject PrevBtn;
    public GameObject NextBtn;
    public List<GameObject> pages;
    private int currentPage;

    void Awake() {
        currentPage = 0;    
    }

    public void NextPage() {

        pages[currentPage].SetActive(false);

        if (currentPage + 1 < pages.Count)
            currentPage++;
        
        if(currentPage+1 >= pages.Count)
            NextBtn.SetActive(false);
        else
            PrevBtn.SetActive(true);

        pages[currentPage].SetActive(true);

    }
    
    public void PreviousPage() {

        pages[currentPage].SetActive(false);

        if(currentPage+1 > 1)
            currentPage--;

        if(currentPage+1 <= 1)
            PrevBtn.SetActive(false);
        else
            NextBtn.SetActive(true);

        pages[currentPage].SetActive(true);

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayButtonController : MonoBehaviour
{

    public GameObject Diamond_continue_button;
    public GameObject Score_continue_button;

    private string PPDiamonds = "Diamonds";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt(PPDiamonds) <= 50)
        {
            Diamond_continue_button.GetComponent<Button>().interactable = false;
        }
        else
        {
            Diamond_continue_button.GetComponent<Button>().interactable = true;
        }
    }

    //Pop Up buttons
    public void Respawn()
    {
        GameManager.instance.GameOver();
        GameManager.instance.HidePopUp();
    }

    public void UseScore()
    {
        GameManager.instance.HidePopUp();
        GameManager.instance.ContinueForScore();
    }

    public void UseDiamond()
    {
        GameManager.instance.HidePopUp();
        GameManager.instance.ContinueForDiamond();
    }
}

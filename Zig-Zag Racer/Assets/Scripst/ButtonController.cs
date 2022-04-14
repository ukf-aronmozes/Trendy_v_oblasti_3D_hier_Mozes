using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public GameObject[] buttons;

    public GameObject garage;
    public GameObject skins;

    public GameObject shopPanel;
    public GameObject commonChestButton;
    public GameObject legendaryChestButton;

    private string PPDiamonds = "Diamonds";
    private string PPPrice = "Price";

    private Color color;

    // Start is called before the first frame update
    private void Awake()
    {
        CurrentCar();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shopPanel.active)
        {
            if (PlayerPrefs.GetInt(PPDiamonds) < 500)
            {
                Debug.Log("asd");
                commonChestButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                commonChestButton.GetComponent<Button>().interactable = true;
            }

            if (PlayerPrefs.GetInt(PPDiamonds) < 1000)
            {
                legendaryChestButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                legendaryChestButton.GetComponent<Button>().interactable = true;
            }
        }
        
    }
    //Start
    public void StartGame()
    {
        GameManager.instance.GameStart();
    }

    //Menu buttons
    public void ShowGarage()
    {
        GameManager.instance.ShowGarage();
    }

    public void ShowMainMenu()
    {
        GameManager.instance.ShowMainMenu();
    }

    public void CloseGarage()
    {
        garage.SetActive(true);
        skins.SetActive(false);
        ShowMainMenu();
    }

    public void ShowShop()
    { 
        GameManager.instance.ShowShop();

    }

    //Car buttons
    public void SaveCar(int i)
    {
        GameManager.instance.SetModel(i);
        ShowSkins(i);
    }

    public void ShowSkins(int i)
    {
        garage.SetActive(false);
        skins.SetActive(true);
        CarSkinButton.instance.SetUP(i);
    }

    public void CurrentCar()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            color = Color.white;

            if (i != GameManager.instance.GetModel())
            {
                if (AvailableCars.instance.GetUnlockedCar(i))
                {
                    color.a = 0.6f;
                    buttons[i].GetComponent<Image>().color = color;
                }
                else
                {
                    buttons[i].GetComponent<Button>().interactable = false;
                }
                
            }
            else
            {
                color.a = 1f;
                buttons[i].GetComponent<Image>().color = color;
            }
        }
    }

    //Shop menu buttons

    public void ChestButton(int price)
    {
        PlayerPrefs.SetInt(PPPrice, price);

        GameManager.instance.ChestPrice(price);
        SceneManager.LoadScene(1);
    }

    public void GetRefund()
    {
        GameManager.instance.ChestPrice(-1 * PlayerPrefs.GetInt(PPPrice)/2);
        GameManager.instance.menuUI[3].SetActive(false);
    }
}

using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool GameStarted;

    public GameObject platforSpawner;
    
    public GameObject gameplayUI;
    public GameObject gameplayUiPopUp;

    public GameObject staticMenuUI;
    public GameObject[] menuUI;

    public GameObject diamond;

    public TextMeshProUGUI scoreNumber;
    public TextMeshProUGUI diamondsNumber;

    public TextMeshProUGUI gameplayScore;
    public TextMeshProUGUI gameplayDiamond;

    AudioSource audioSource;
    public AudioClip[] gameMusic;

    public int score = 0;
    public int scoreD;
    public int scoreAmount = 1;
    public int diamondAmount = 1;
    private int highScore;
    public int currentMenuScreen = 0;
    private int powerUpID = 0;

    [SerializeField] int model_number;

    private string PPHighScore = "HighScore";
    private string PPDiamonds = "Diamonds";
    private string PPCar = "Car";
    private string PPCarSkin = "CarSkin";
    private string PPPowerUp = "PowerUp";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt(PPDiamonds, 2000);
        highScore = PlayerPrefs.GetInt(PPHighScore);
        scoreD = PlayerPrefs.GetInt(PPDiamonds);
        SetMenu();
        CarProperties();
        ScreenCapture.CaptureScreenshot("Assets/mainMenu.png", 1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameStart()
    {
        GameStarted = true; 
        platforSpawner.SetActive(true);
        gameplayUI.SetActive(true);
        gameplayUiPopUp.SetActive(false);

        audioSource.clip = gameMusic[1];
        audioSource.Play();

        staticMenuUI.SetActive(false);
        menuUI[0].SetActive(false);
        currentMenuScreen = 0;

        Destroy(diamond);

        gameplayDiamond.SetText(":" + scoreD.ToString());
        gameplayScore.SetText(":" + score.ToString());

        StartCoroutine(nameof(UpdateScore));
    }

    public void GameOver()
    {
        platforSpawner.SetActive(false);

        SaveHighScore();
        Debug.Log("asd");
        StopCoroutine(nameof(UpdateScore));
        SetMenu();
        ReloadLevel();
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene("GameScene");

        gameplayUI.SetActive(false);

        staticMenuUI.SetActive(true);
        menuUI[0].SetActive(true);

        SetModel(GetModel());
    }

    public void UpdateScore()
    {
        score += scoreAmount;

        gameplayScore.SetText(":" + score.ToString());
    }

    public void UpdateDiamond()
    {
        scoreD += diamondAmount;

        gameplayDiamond.SetText(":" + scoreD.ToString());

        audioSource.PlayOneShot(gameMusic[2], 0.1f);

        if (PlayerPrefs.HasKey(PPDiamonds))
        {
            PlayerPrefs.SetInt(PPDiamonds, scoreD);
            scoreD = PlayerPrefs.GetInt(PPDiamonds);
        }
        else
        {
            PlayerPrefs.SetInt(PPDiamonds, scoreD);
        }
    }

    void SaveHighScore()
    {
        if (PlayerPrefs.HasKey(PPHighScore))
        {
            if (score > PlayerPrefs.GetInt(PPHighScore))
            {
                PlayerPrefs.SetInt(PPHighScore, score);
                highScore = PlayerPrefs.GetInt(PPHighScore);
            }

        }
        else
        {
            PlayerPrefs.SetInt(PPHighScore, score);
        }
    }

    void SetMenu()
    {
        scoreNumber.SetText(highScore.ToString());
        diamondsNumber.SetText(scoreD.ToString());
    }

    //Speed Up sound
    public void PlaySpeedUpSound()
    {
        audioSource.PlayOneShot(gameMusic[3], 0.7f);
    }

    //Car
    public int GetModel()
    {
        return PlayerPrefs.GetInt(PPCar);
    }

    public void SetModel(int model_number)
    {
        if (this.model_number != model_number)
        {
            SaveCarSkin(0);
        }

        this.model_number = model_number;
        SaveCarModel();
        CarProperties();
        CarController.instance.SetModel();
    }

    public void SaveCarModel()
    {
        PlayerPrefs.SetInt(PPCar, model_number);
    }

    public void SaveCarSkin(int index)
    {
        PlayerPrefs.SetInt(PPCarSkin, index);
    }

    public void CarProperties()
    {
        scoreAmount = 1;
        diamondAmount = 1;
        switch (PlayerPrefs.GetInt(PPCar))
        {
            case 0:
                PlayerPrefs.SetInt(PPPowerUp, 0);
                break;
            case 1:
                PlayerPrefs.SetInt(PPPowerUp, 0);
                break;
            case 2:
                scoreAmount = 2;
                PlayerPrefs.SetInt(PPPowerUp, 1);
                break;
            case 3:
                diamondAmount = 2;
                PlayerPrefs.SetInt(PPPowerUp, 2);
                break;
            case 4:
                scoreAmount = 5;
                PlayerPrefs.SetInt(PPPowerUp, 3);
                break;
            case 5:
                diamondAmount = 5;
                PlayerPrefs.SetInt(PPPowerUp, 4);
                break;
        }
    }

    //Main menu
    public void ShowMainMenu()
    {
        menuUI[currentMenuScreen].SetActive(false);
        currentMenuScreen = 0;
        menuUI[currentMenuScreen].SetActive(true);
    }

    //Garage
    public void ShowGarage()
    {
        menuUI[currentMenuScreen].SetActive(false);
        currentMenuScreen = 1;
        menuUI[currentMenuScreen].SetActive(true);
    }

    //Shop
    public void ShowShop()
    {
        menuUI[currentMenuScreen].SetActive(false);
        currentMenuScreen = 2;
        menuUI[currentMenuScreen].SetActive(true);
    }

    //Pop Up
    public void ShowPopUp()
    {
        platforSpawner.SetActive(false);
        CitySpawner.instance.delete = false;
        gameplayUiPopUp.SetActive(true);
    }

    public void HidePopUp()
    {
        CitySpawner.instance.delete = true;
        gameplayUiPopUp.SetActive(false);
    }
    
    public void ContinueForScore()
    {
        score /= 2;
        gameplayScore.SetText(score.ToString());

        CarController.instance.RespawnCar();

        platforSpawner.SetActive(true);
        Platform_Spawner.instance.Restart();
    }

    public void ContinueForDiamond()
    {
        scoreD -= 50;

        gameplayDiamond.SetText(scoreD.ToString());

        if (PlayerPrefs.HasKey(PPDiamonds))
        {
            PlayerPrefs.SetInt(PPDiamonds, scoreD);
            scoreD = PlayerPrefs.GetInt(PPDiamonds);
        }
        else
        {
            PlayerPrefs.SetInt(PPDiamonds, scoreD);
        }

        CarController.instance.RespawnCar();

        platforSpawner.SetActive(true);
        Platform_Spawner.instance.Restart();
    }

    public void ChestPrice(int price)
    {
        scoreD -= price;

        if (PlayerPrefs.HasKey(PPDiamonds))
        {
            PlayerPrefs.SetInt(PPDiamonds, scoreD);
            scoreD = PlayerPrefs.GetInt(PPDiamonds);
        }
        else
        {
            PlayerPrefs.SetInt(PPDiamonds, scoreD);
        }

        SetMenu();
    }

    public static implicit operator GameManager(Platform_Spawner v)
    {
        throw new NotImplementedException();
    }
}
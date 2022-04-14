using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShowPowerUp : MonoBehaviour
{
    public static ShowPowerUp instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public GameObject[] pU;

    private string PPPowerUp = "PowerUp";

    void Start()
    {
        for (int i = 0; i < pU.Length; i++)
        {
            pU[i].SetActive(false);
        }
        ChangePowerUp(PlayerPrefs.GetInt(PPPowerUp));
    }

    public void ChangePowerUp(int index)
    {
        for (int i = 0; i < pU.Length; i++)
        {
            if (index == i)
            {
                pU[i].SetActive(true);
            }
            else
            {
                pU[i].SetActive(false);
            }
        }
    }
}

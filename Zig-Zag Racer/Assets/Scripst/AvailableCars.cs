using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableCars : MonoBehaviour
{
    public static AvailableCars instance;

    public bool[] availabled;

    private static string PPCarAva = "availableCars";
    private static string PPSkinAva = "availableSkins";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(this);
    }

    public void Start()
    {
        GetPPCar();
        availabled[0] = true;
        StePPCar();
    }

    public void UnlockCar(int carIndex, int skinIndex)
    {
        if (!CarSkinButton.instance.carImages[carIndex].avaiable[skinIndex])
        {
            availabled[carIndex] = true;
            StePPCar();

            UnlockSkin(carIndex, skinIndex);
        }
        else
        {
           StartCoroutine(PopUpFun());
        }
    }

    IEnumerator PopUpFun()
    {
        yield return new WaitForSeconds(0.0f);
        GameManager.instance.menuUI[3].SetActive(true);
        StopCoroutine(PopUpFun());
    }

    public bool GetUnlockedCar(int index)
    {
        return availabled[index];
    }

    public void StePPCar()
    {
        for (int i = 0; i < availabled.Length; i++)
        {
            PlayerPrefs.SetInt(PPCarAva + i, availabled[i] ? 1 : 0);
            //PlayerPrefs.SetInt(PPCarAva + i, 1);
        }
    }

    public void GetPPCar()
    {
        for (int i = 0; i < availabled.Length; i++)
        {
            availabled[i] = PlayerPrefs.GetInt(PPCarAva + i) == 1 ? true : false;
        }
    }

    public void UnlockSkin(int carIndex, int skinIndex)
    {
        CarSkinButton.instance.carImages[carIndex].avaiable[skinIndex] = true;
        SetPPSkin(carIndex);
    }

    public void SetPPSkin(int carIndex)
    {
        var array = CarSkinButton.instance.carImages[carIndex].avaiable;
        for (int i = 0; i < array.Length; i++)
        {
            PlayerPrefs.SetInt(PPSkinAva + carIndex + i, array[i] ? 1 : 0);
        }
    }

    public void GetPPSkin(int carIndex)
    {
        if (PlayerPrefs.HasKey(PPSkinAva + carIndex + 0))
        {
            var array = CarSkinButton.instance.carImages[carIndex].avaiable;
            for (int i = 0; i < array.Length; i++)
            {
                Debug.Log(PlayerPrefs.GetInt(PPSkinAva + carIndex + i));
                array[i] = PlayerPrefs.GetInt(PPSkinAva + carIndex + i) == 1 ? true : false;
            }
        }
    }
}
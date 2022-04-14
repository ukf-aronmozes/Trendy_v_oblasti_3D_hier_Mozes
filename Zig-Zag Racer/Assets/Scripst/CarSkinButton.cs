using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSkinButton : MonoBehaviour
{
    public static CarSkinButton instance;

    public GameObject[] buttons;

    [Serializable]
    public class ButtonImages
    {
        public Sprite bg;
        public Sprite[] images;
        public bool[] avaiable;
    }

    public ButtonImages[] carImages;

    private static int carIndex;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        
    }

    public void SetUP(int index)
    {
        carIndex = index;

        AvailableCars.instance.GetPPSkin(index);

        for (int i = 0; i < 5; i++)
        {
            buttons[i].GetComponent<Image>().sprite = carImages[index].bg;
            Transform currentButton = buttons[i].transform.GetChild(0);
            currentButton.GetComponent<Image>().sprite = carImages[index].images[i];
            currentButton.GetComponent<Image>().SetNativeSize();
            if (!carImages[index].avaiable[i])
            {
                buttons[i].GetComponent<Button>().interactable = false;
            }
            else
            {
                buttons[i].GetComponent<Button>().interactable = true;
            }
        }

        if (!carImages[index].avaiable[0])
        {
            for (int i = 0; i < 5; i++)
            {
                if (carImages[index].avaiable[i])
                {
                    SetSkin(i);
                    break;
                }
            }
        }
    }

    public void SetSkin(int index)
    {
        int changeIndex = 0;

        if (carIndex == 0 || carIndex == 1 || carIndex == 3 || carIndex == 5)
        {
            changeIndex = 1;
        }
        else if (carIndex == 2 ||  carIndex == 4)
        {
            changeIndex = 0;
        }

        var sprite = CarSkins.instance.sprite[carIndex];
        Renderer ren = sprite.GetComponent<Renderer>();
        Material[] mat = ren.materials;

        mat[changeIndex] = CarSkins.instance.cars[carIndex].skins[index];

        ren.materials = mat;

        GameManager.instance.SaveCarSkin(index);
    }

    public void SetSkinWithCar(int car, int skin)
    {
        int changeIndex = 0;

        if (car == 0 || car == 1 || car == 3 || car == 5)
        {
            changeIndex = 1;
        }
        else if (car == 2 || car == 4)
        {
            changeIndex = 0;
        }

        var sprite = CarSkins.instance.sprite[car];
        Renderer ren = sprite.GetComponent<Renderer>();
        Material[] mat = ren.materials;
        
        mat[changeIndex] = CarSkins.instance.cars[car].skins[skin];
      
        ren.materials = mat;
    }   
}

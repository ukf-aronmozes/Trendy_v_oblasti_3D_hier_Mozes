using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSkins : MonoBehaviour
{
    public static CarSkins instance;

    [Serializable]
    public class CarMaterials
    {
        public Material[] skins;
    }
    public CarMaterials[] cars;

    public GameObject[] sprite;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        
    }
}

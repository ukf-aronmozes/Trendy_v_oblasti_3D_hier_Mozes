using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public static CarController instance;

    public GameObject pickUpEffect;
    public GameObject speedUpEffect;

    public float moveSpeed;
    public float moveSpeedHelp;
    public float speedBoostMoveSpeed;
    public float restartMoveSpeed;

    public Vector3 respawnPos;

    public int speedUpduration = 10;

    public GameObject[] models;

    private bool movingLeft = true;
    private bool firstInput = true;
    public bool active = false;

    private string tagDiamond = "Diamond";
    private string tagSu = "SpeedUp";
    private string tagPlatform = "Platform";
    private string PPCarSkin = "CarSkin";

    private string PPCar = "Car";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetModel();
    }

    // Update is called once per frame
    void Update()
    {        
        if (GameManager.instance.GameStarted)
        {
            Move();
            CheckInput();
        }
        
        if (transform.position.y <= -10)
        {
            ShowPopUp();
        }

        if (GameManager.instance.GameStarted)
        {
            Platform_Spawner.instance.WaitTime = (moveSpeed / Mathf.Pow(moveSpeed, 2)) * 1.75f;
        }        
    }

    public void ShowPopUp()
    {
        GameManager.instance.ShowPopUp();
        GetComponent<Rigidbody>().isKinematic = true;
        GameManager.instance.GameStarted = false;
    }

    void Move()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    public void RespawnCar()
    {
        Camera_Follow.instance.ResetCamera();

        GameManager.instance.GameStarted = true;

        GetComponent<Rigidbody>().isKinematic = false;
        respawnPos = (Vector3)Platform_Spawner.instance.platformPos[0];
        respawnPos.y = 1f;
        transform.position = respawnPos;
        moveSpeedHelp = 0;

        if (!active)
        {
            restartMoveSpeed = moveSpeed;
        }
        
        SetMoveSpeed(0);
        StartCoroutine(nameof(SpeedUpAfterRestart));

        Platform_Spawner.instance.platformPos.RemoveAt(0);        
    }

    void CheckInput()
    {
        if (firstInput)
        {
            firstInput = false;
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            ChangeDirection();
        }
    }

    void ChangeDirection()
    {
        if (movingLeft)
        {
            movingLeft = false;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            movingLeft = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == tagDiamond)
        {
            other.gameObject.SetActive(false);

            Destroy(other);

            Vector3 effectPosition = other.transform.position;
            effectPosition.y = 0.5f;

            Instantiate(pickUpEffect, effectPosition, pickUpEffect.transform.rotation);

            GameManager.instance.UpdateDiamond();            
        }
        if (other.gameObject.tag == tagSu)
        {
            other.gameObject.SetActive(false);

            Destroy(other);

            Vector3 effectPosition = other.transform.position;
            effectPosition.y = 0.5f;

            Instantiate(speedUpEffect, effectPosition, speedUpEffect.transform.rotation);

            GameManager.instance.PlaySpeedUpSound();

            restartMoveSpeed = moveSpeed;

            SetMoveSpeed(moveSpeed * 2);

            speedBoostMoveSpeed = moveSpeed;

            active = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (active)
        {
            if (collision.gameObject.tag == tagPlatform)
            {

                speedUpduration--;

                if (speedUpduration == 3)
                {
                    
                    SetMoveSpeed(moveSpeed - 1);
                }
                else if (speedUpduration == 2)
                {
                    
                    SetMoveSpeed(moveSpeed - 2);
                }
                else if (speedUpduration == 1)
                {
                    
                    SetMoveSpeed(moveSpeed - 3);
                }
                else if (speedUpduration == 0)
                {
                    speedUpduration = 10;
                    active = false;
                    SetMoveSpeed(speedBoostMoveSpeed/2);
                }
            }
        }
        if (GameManager.instance.GameStarted && collision.gameObject.tag == tagPlatform)
        {
            SetMoveSpeed(moveSpeed * 1.005f);
            GameManager.instance.UpdateScore();
        }
    }

    public void SetModel()
    {
        int car = PlayerPrefs.GetInt(PPCar);
        int skin = PlayerPrefs.GetInt(PPCarSkin);

        for (int i = 0; i < models.Length; i++)
        {
            models[i].SetActive(false);
        }

        models[GameManager.instance.GetModel()].SetActive(true);
        CarSkinButton.instance.SetSkinWithCar(car, skin);
    }

    public void SetMoveSpeed(float i)
    {
        moveSpeed = i;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    IEnumerator SpeedUpAfterRestart()
    {
        while (moveSpeedHelp <= restartMoveSpeed)
        {
            SetMoveSpeed(GetMoveSpeed() + 0.5f);
            moveSpeedHelp += 0.5f;

            yield return new WaitForSeconds(0.1f);
        }
    }
}

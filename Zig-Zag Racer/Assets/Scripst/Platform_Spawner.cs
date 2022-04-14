using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Platform_Spawner : MonoBehaviour
{
    public static Platform_Spawner instance;

    public GameObject platform;
    public GameObject diamond;

    public Transform lastPlatform;

    public float WaitTime;
    private float WaitTimeHelp;

    Vector3 lastPosition;
    Vector3 newPosition;

    private int lastInt;

    public  bool spawn = true;
    private bool stop;
    private bool boost = false;

    private int boostLength = 10;
    private int spawCity = 10;

    public ArrayList platformPos = new ArrayList();


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
        lastPosition = lastPlatform.position;
        platformPos.Add(lastPosition);

        StartCoroutine(nameof(SpawnNewPlatforms));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Restart()
    {
        platformPos.Add(lastPosition);

        StartCoroutine(nameof(SpawnNewPlatforms));
    }

    IEnumerator SpawnNewPlatforms()
    {
        while (!stop)
        {
            platformPos.Add(lastPosition);
            if (!boost)
            {
                int random = Random.Range(0, 100);

                if (random > 95)
                {
                    boost = true;
                }
                else
                {
                    boost = false;
                }
            }

            newPosition = lastPosition;

            if (boost)
            {
                GeneretSpeedPosition();
            }
            else
            {
                GenerateNewPosition();
            }

            Instantiate(platform, newPosition, Quaternion.identity);

            if (spawCity == 0)
            {
                CitySpawner.instance.SpawnCity(newPosition);
                spawCity = 10;
            }

            spawCity--;

            if (spawn)
            {
                Diamond.instance.SpawnDiamond(lastPosition);
            }

            lastPosition = newPosition;
            

            yield return new WaitForSeconds(WaitTime);
        }
    }


    void GenerateNewPosition()
    {
        int rand = Random.Range(0, 2);
        lastInt = rand;

        if (rand == 0)
        {
            newPosition.x += 2f;
        }
        else
        {
            newPosition.z += 2f;
        }
    }

    void GeneretSpeedPosition()
    {
        if (boostLength == 10 && boost)
        {
            SpeedBoost.instance.SpawnBoost(newPosition);
            spawn = false;
        }
        else
        {
            spawn = true;
        }

        boostLength--;

        if (boostLength == 0)
        {
            boostLength = 10;
            boost = false;
        }

        if (lastInt == 0)
        {
            newPosition.x += 2f;
        }
        else
        {
            newPosition.z += 2f;
        }
    }


}

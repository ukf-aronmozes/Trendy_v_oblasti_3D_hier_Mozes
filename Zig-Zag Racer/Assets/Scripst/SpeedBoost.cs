using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public static SpeedBoost instance;

    public GameObject speedUp;

    private Vector3 speedCoinPosition;

    private string tagCar = "Player";

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == tagCar)
        {
            DestroyImmediate(gameObject);
        }
    }

    public void SpawnBoost(Vector3 position)
    {

        if (speedUp != null)
        {
            position.y += 1.2f;
            Instantiate(speedUp, position, speedUp.transform.rotation);
        }
    }
}